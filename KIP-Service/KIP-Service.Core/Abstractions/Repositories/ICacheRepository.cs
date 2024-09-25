using KIP_Service.Core.Models;

namespace KIP_Service.DataAccess.Repositories
{
    public interface ICacheRepository
    {
        Task<Guid> AddAsync<T>(Guid queryId, QueryCache<T> queryCache);
        Task<QueryCache<T>?> GetAsync<T>(Guid queryId);
    }
}