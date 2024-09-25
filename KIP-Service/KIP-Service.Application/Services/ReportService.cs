using CSharpFunctionalExtensions;
using KIP_Service.Core.Models;
using KIP_Service.DataAccess.Repositories;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace KIP_Service.Application.Services
{
    public class ReportService(
        IUserStatisticRepository userStatisticRepository,
        ICacheRepository cacheRepository,
        IConfiguration configuration) : IReportService
    {
        private readonly IUserStatisticRepository _userStatisticRepository = userStatisticRepository;
        private readonly ICacheRepository _cacheRepository = cacheRepository;
        private readonly int ExpectedSeconds = int.Parse(configuration.GetSection(nameof(ExpectedSeconds)).Value);

        public async Task<Guid> GetUserStatisticAsync(Guid userId, DateTime from, DateTime to)
        {
            var requestStatistic = new RequestStatistic(userId, from, to);

            var queryCache = new QueryCache<RequestStatistic>(
                nameof(GetUserStatisticAsync),
                DateTime.Now,
                requestStatistic);

            return await _cacheRepository.AddAsync(
                Guid.NewGuid(),
                queryCache);
        }

        public async Task<Result<QueryInfo<UserStatistic>>> GetQueryInfoAsync(Guid queryId)
        {
            var queryCache = await _cacheRepository.GetAsync<RequestStatistic>(queryId);

            if (queryCache == null)
                return Result.Failure<QueryInfo<UserStatistic>>("Query has not been found");

            var percent = (int)((DateTime.Now - queryCache.CreatedDate).TotalSeconds / ExpectedSeconds * 100);

            UserStatistic? userStatistic = null;

            if (percent >= 100)
            {
                var result = await _userStatisticRepository.GetAsync(queryCache.QueryDetails);

                percent = 100;

                if (result.IsSuccess)
                    userStatistic = result.Value;
            }

            return new QueryInfo<UserStatistic>(
                queryId,
                percent,
                userStatistic);
        }
    }
}
