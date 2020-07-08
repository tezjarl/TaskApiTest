using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using TaskApiTest.Constants;

namespace TaskApiTest.Models
{
    public class Job
    {
        [Key]
        [JsonIgnore]
        public Guid Id { get; private set; }
        public string Status { get; private set; }
        public DateTime Timestamp { get; private set; }
        
        public Job(Guid id)
        {
            Id = id;
            Timestamp = DateTime.UtcNow;
            Status = JobStatus.CREATED;
        }

        public void SetRunning()
        {
            Timestamp = DateTime.UtcNow;
            Status = JobStatus.RUNNING;
        }

        public void SetFinished()
        {
            Timestamp = DateTime.UtcNow;
            Status = JobStatus.FINISHED;
        }
    }
}