using CSharpFunctionalExtensions;
using KIP_Service.Core.Models;

namespace KIP_Service.DataAccess.Repositories
{
    public interface IUserStatisticRepository
    {
        Task<Result> AddSinginAsync(UserSingIn userSingIn);
        Task<Result<UserStatistic>> GetAsync(RequestStatistic requestStatistic);
    }
}