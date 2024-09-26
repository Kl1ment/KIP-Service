using KIP_Service.Core.Models;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Text.Json;

namespace KIP_Service.DataAccess.Repositories
{
    public class CacheRepository(
        IDistributedCache cache,
        RedisContext redisContext) : ICacheRepository
    {
        private readonly IDistributedCache _cache = cache;

        public async Task<QueryCache<T, A>?> GetAsync<T, A>(Guid queryId)
        {
            var request = await _cache.GetStringAsync(queryId.ToString());

            if (request == null)
                return null;

            return JsonSerializer.Deserialize<QueryCache<T, A>>(request);
        }

        public async Task<string?> GetStringAsync(string str)
        {
            return await _cache.GetStringAsync(str);
        }

        public RedisKey[] GetKeys()
        {
            var endPoint = redisContext.Connection.GetEndPoints().First();

            var keys = redisContext.Connection.GetServer(endPoint).Keys(pattern: "*").ToArray();

            return keys;
        }

        public async Task SetCacheAsync<T, A>(Guid queryId, QueryCache<T, A> queryCache)
        {
            await _cache.SetStringAsync(
                queryId.ToString(),
                JsonSerializer.Serialize(queryCache));
        }

    }
}
