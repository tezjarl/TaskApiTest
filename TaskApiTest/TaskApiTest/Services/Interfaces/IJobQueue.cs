using System.Threading;
using TaskApiTest.Models;

namespace TaskApiTest.Services.Interfaces
{
    public interface IJobQueue
    {
        void Enqueue(Job job);
        Job Dequeue(CancellationToken token);
    }
}