using System;
using System.Threading;
using System.Threading.Tasks;
using TaskApiTest.Models;

namespace TaskApiTest.Services.Interfaces
{
    public interface IJobManager
    {
        Task<Job> GetTaskStatus(Guid id, CancellationToken token);
        Task<Guid> CreateTask(CancellationToken token);
    }
}