using CSharpFunctionalExtensions;
using KIP_Service.Core.Models;

namespace KIP_Service.DataAccess.Repositories
{
    public interface IUserRepository
    {
        Task<Result<Guid>> AddAsync(User user);
    }
}