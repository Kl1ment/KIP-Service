using KIP_Service.Core.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace KIP_Service.DataAccess.Repositories
{
    public class CacheRepository(IDistributedCache cache) : ICacheRepository
    {
        private readonly IDistributedCache _cache = cache;

        public async Task<QueryCache<T, A>?> GetAsync<T, A>(Guid queryId)
        {
            var request = await _cache.GetStringAsync(queryId.ToString());

            if (request == null)
                return null;

            return JsonSerializer.Deserialize<QueryCache<T, A>>(request);
        }

        public async Task SetCacheAsync<T, A>(Guid queryId, QueryCache<T, A> queryCache)
        {
            await _cache.SetStringAsync(
                queryId.ToString(),
                JsonSerializer.Serialize(queryCache));
        }
    }
}
