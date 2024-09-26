using KIP_Service.Core.Models;
using StackExchange.Redis;

namespace KIP_Service.DataAccess.Repositories
{
    public interface ICacheRepository
    {
        Task<QueryCache<T, A>?> GetAsync<T, A>(Guid queryId);
        Task<string?> GetStringAsync(string str);
        RedisKey[] GetKeys();
        Task SetCacheAsync<T, A>(Guid queryId, QueryCache<T, A> queryCache);
    }
}