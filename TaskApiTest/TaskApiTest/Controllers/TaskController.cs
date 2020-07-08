using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskApiTest.Models;
using TaskApiTest.Services.Interfaces;

namespace TaskApiTest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly IJobManager _jobManager;
        private const string EMPTY_GUID = "No guid provided";
        private const string INVALID_GUID = "Provided string is not a guid";
        private const string NO_RESULT = "No task with guid {0} was found";
        
        public TaskController(IJobManager jobManager)
        {
            _jobManager = jobManager;
        }
        /// <summary>
        /// Get task status by its id
        /// </summary>
        /// <param name="guid">GUID</param>
        /// <returns>Object which describes the status of the task</returns>
        /// <response code="200">Returns the task object</response>
        /// <response code="400">If provided string is not a valid guid</response>
        /// <response code="404">If task with provided guid doesn't exist in database</response>
        [HttpGet]
        [Route("{guid}")]
        [ProducesResponseType(typeof(Job),(int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(string guid, CancellationToken token)
        {
            if (string.IsNullOrEmpty(guid))
            {
                return BadRequest(EMPTY_GUID);
            }

            if (!Guid.TryParse(guid, out var taskId))
            {
                return BadRequest(INVALID_GUID);
            }

            var task = await _jobManager.GetTaskStatus(taskId, token);
            if (task == null)
            {
                return NotFound(string.Format(NO_RESULT, guid));
            }
            return Ok(task);
        }
        /// <summary>
        /// Create new task and run it in background
        /// </summary>
        /// <returns>Guid of the created task</returns>
        /// <response code="202">Guid of the created task</response>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.Accepted)]
        public async Task<IActionResult> Post(CancellationToken token)
        {
            var createdTask = await _jobManager.CreateTask(token);
            return Accepted(createdTask);
        }
    }
}