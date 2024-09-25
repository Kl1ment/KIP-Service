using CSharpFunctionalExtensions;
using KIP_Service.Core.Models;

namespace KIP_Service.Application.Services
{
    public interface IReportService
    {
        Task<Result<QueryInfo<UserStatistic>>> GetQueryInfoAsync(Guid queryId);
        Task<Guid> GetUserStatisticAsync(Guid userId, DateTime from, DateTime to);
    }
}