using Microsoft.Extensions.Caching.Memory;
using web_api.Jobs;
using web_api.Model;

namespace web_api.Services
{
    public class CachedIPService : IIPService
    {
        private readonly IIPService _decorated;
        private readonly IMemoryCache _memoryCache;

        public CachedIPService(IPServiceImpl decorated, IMemoryCache memoryCache)
        {
            _decorated = decorated;
            _memoryCache = memoryCache;
        }

        public Task<IPInfoEntity> GetIpDetails(string ip)
        {
            return _memoryCache.GetOrCreateAsync<IPInfoEntity>(
                ip,
                entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));

                    return _decorated.GetIpDetails(ip);
                })!;
        }

        public void SetCompletedJobInfo(JobDTO job)
        {
            _memoryCache.GetOrCreate<JobDTO>(
                job.Id, 
                entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

                    return job;
                });
        }

        public Guid CreateNewUpdateJob(Queue<IPInfoEntity> entities)
        {
            return _decorated.CreateNewUpdateJob(entities);
        }

        public JobDTO GetJobInfo(Guid id)
        {
            if (_memoryCache.TryGetValue(id, out var job))
            {
                return (JobDTO)job!;
            }

            return _decorated.GetJobInfo(id);
        }

        public Task<List<IPInfoEntity>> UpdateIpDetails(List<IPInfoEntity> entities)
        {
            return _decorated.UpdateIpDetails(entities);
        }
    }
}
