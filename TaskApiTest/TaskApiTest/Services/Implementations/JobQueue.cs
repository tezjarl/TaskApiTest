using System.Collections.Concurrent;
using System.Threading;
using TaskApiTest.Models;
using TaskApiTest.Services.Interfaces;

namespace TaskApiTest.Services.Implementations
{
    public class JobQueue: IJobQueue
    {
        private readonly BlockingCollection<Job> _jobs;

        public JobQueue() => _jobs = new BlockingCollection<Job>();

        public void Enqueue(Job job) => _jobs.Add(job);

        public Job Dequeue(CancellationToken token) => _jobs.Take(token);
    }
}