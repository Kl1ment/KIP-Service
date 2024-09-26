using KIP_Service.Core.Models;

namespace KIP_Service.DataAccess.Repositories
{
    public interface ICacheRepository
    {
        Task SetCacheAsync<T, A>(Guid queryId, QueryCache<T, A> queryCache);
        Task<QueryCache<T, A>?> GetAsync<T, A>(Guid queryId);
    }
}