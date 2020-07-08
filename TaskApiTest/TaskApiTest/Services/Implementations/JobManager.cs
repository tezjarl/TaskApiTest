using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskApiTest.Models;
using TaskApiTest.Repositories.Interfaces;
using TaskApiTest.Services.Interfaces;

namespace TaskApiTest.Services.Implementations
{
    public class JobManager: IJobManager
    {
        private IRepository<Job> _repository;
        private readonly IJobQueue _queue;
        private readonly ILogger<JobManager> _logger;

        public JobManager(IRepository<Job> repository, IJobQueue queue, ILogger<JobManager> logger)
        {
            _repository = repository;
            _queue = queue;
            _logger = logger;
        }
        public async Task<Job> GetTaskStatus(Guid id, CancellationToken token)
        {
            return await _repository.GetById(id, token);
        }

        public async Task<Guid> CreateTask(CancellationToken token)
        { 
            var taskId = Guid.NewGuid();
            var newTask = new Job(taskId);
            await _repository.Add(newTask, token);
            _logger.LogInformation($"Task {taskId} was created");
            _queue.Enqueue(newTask);
            
            return taskId;
        }

        
    }
}