using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskApiTest.Models;
using TaskApiTest.Repositories.Interfaces;

namespace TaskApiTest.Repositories.Implementations
{
    public class JobRepository: IRepository<Job>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public JobRepository(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        public async Task<Job> GetById(Guid id, CancellationToken token)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<JobDbContext>();
                return await db.TaskResults.FirstOrDefaultAsync(task => task.Id == id, token);
            }
        }

        public async Task Add(Job newItem, CancellationToken token)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<JobDbContext>();
                db.TaskResults.Add(newItem);
                await db.SaveChangesAsync(token);
            }
        }

        public async Task Update(Job updatedItem, CancellationToken token)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<JobDbContext>();
                db.TaskResults.Update(updatedItem);
                await db.SaveChangesAsync(token);
            }
        }
    }
}