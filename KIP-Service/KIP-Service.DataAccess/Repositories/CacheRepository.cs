using KIP_Service.Core.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace KIP_Service.DataAccess.Repositories
{
    public class CacheRepository(IDistributedCache cache) : ICacheRepository
    {
        private readonly IDistributedCache _cache = cache;

        public async Task<QueryCache<T>?> GetAsync<T>(Guid queryId)
        {
            var request = await _cache.GetStringAsync(queryId.ToString());

            if (request == null)
                return null;

            return JsonSerializer.Deserialize<QueryCache<T>>(request);
        }

        public async Task<Guid> AddAsync<T>(Guid queryId, QueryCache<T> queryCache)
        {
            await _cache.SetStringAsync(
                queryId.ToString(),
                JsonSerializer.Serialize(queryCache));

            return queryId;
        }
    }
}
