using CSharpFunctionalExtensions;
using KIP_Service.Core.Models;

namespace KIP_Service.Application.Services
{
    public interface IReportService
    {
        Task<Result<QueryInfo<UserStatistic>>> GetQueryInfoAsync(Guid queryId);
        Guid GetUserStatistic(Guid userId, DateTime from, DateTime to);
        void ExecuteGetUserStatisticAsync(QueryCache<RequestStatistic, UserStatistic> queryCache);
    }
}