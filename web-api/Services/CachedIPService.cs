using Microsoft.Extensions.Caching.Memory;
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

        public Task<List<IPInfoEntity>> GetAllIpDetails()
        {
            return _decorated.GetAllIpDetails();
        }

        public Task<IPInfoEntity> GetIpDetails(string ip)
        {
            return _memoryCache.GetOrCreateAsync(
                ip,
                entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(1));

                    return _decorated.GetIpDetails(ip);
                })!;
        }
    }
}
