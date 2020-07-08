using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TaskApiTest.Models;
using TaskApiTest.Repositories.Interfaces;
using TaskApiTest.Services.Interfaces;

namespace TaskApiTest.Services.Implementations
{
    public class FakeJobWorker: IHostedService
    {
        private readonly IRepository<Job> _repository;
        private readonly IJobQueue _queue;
        private readonly ILogger<JobManager> _logger;
        private CancellationTokenSource _cancellationTokenSource;
        private const int LONG_RUNNING_TIME = 120_000; // 2 minutes

        public FakeJobWorker(IRepository<Job> repository, IJobQueue queue, ILogger<JobManager> logger)
        {
            _repository = repository;
            _queue = queue;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource= CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            while (cancellationToken.IsCancellationRequested == false)
            {
                try
                {
                    var nextJob = _queue.Dequeue(cancellationToken);
                    nextJob.SetRunning();
                    await _repository.Update(nextJob, cancellationToken);
                    _logger.LogInformation($"Task {nextJob.Id} run");
                
                    await Task.Delay(LONG_RUNNING_TIME, cancellationToken);
                
                    nextJob.SetFinished();
                    await _repository.Update(nextJob, cancellationToken);
                    _logger.LogInformation($"Task {nextJob.Id} was finished");
                }
                catch (OperationCanceledException e)
                {
                    _logger.LogWarning("Operation was cancelled");
                    throw;
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource.Cancel();
            return Task.CompletedTask;
        }
    }
}