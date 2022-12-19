using System.Collections.Concurrent;
using web_api.Model;

namespace web_api.Jobs
{
    public class BackgroundJobs
    {
        public SortedDictionary<Guid, JobDTO> QueuedTasks { get; set; } = new();
        public Dictionary<Guid, JobDTO> CompletedTasks { get; set; } = new();
    }
}
