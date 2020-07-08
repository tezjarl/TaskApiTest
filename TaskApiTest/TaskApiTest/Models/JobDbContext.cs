using Microsoft.EntityFrameworkCore;

namespace TaskApiTest.Models
{
    public class JobDbContext: DbContext
    {
        public DbSet<Job> TaskResults { get; set; }

        public JobDbContext(DbContextOptions<JobDbContext> options): base(options)
        {
            Database.EnsureCreated();
        }
    }
}