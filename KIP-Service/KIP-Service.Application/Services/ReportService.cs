using CSharpFunctionalExtensions;
using KIP_Service.Core.Models;
using KIP_Service.DataAccess.Repositories;
using Microsoft.Extensions.Configuration;

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

        public Guid GetUserStatistic(Guid userId, DateTime from, DateTime to)
        {
            var requestStatistic = new RequestStatistic(userId, from, to);
            
            var queryCache = new QueryCache<RequestStatistic, UserStatistic>(
                Guid.NewGuid(),
                nameof(GetUserStatistic),
                DateTime.Now,
                requestStatistic);

            ExecuteGetUserStatisticAsync(queryCache);

            return queryCache.Id;
        }

        public async Task<Result<QueryInfo<UserStatistic>>> GetQueryInfoAsync(Guid queryId)
        {
            var queryCache = await _cacheRepository.GetAsync<RequestStatistic, UserStatistic>(queryId);

            if (queryCache == null)
                return Result.Failure<QueryInfo<UserStatistic>>("Query has not been found");

            var percent = Math.Min(
                (int)((DateTime.Now - queryCache.CreatedDate).TotalSeconds / ExpectedSeconds * 100),
                100);

            return new QueryInfo<UserStatistic>(
                queryId,
                percent,
                queryCache.Result);
        }

        public async void ExecuteGetUserStatisticAsync(QueryCache<RequestStatistic, UserStatistic> queryCache)
        {
            await _cacheRepository.SetCacheAsync(
                queryCache.Id,
                queryCache);

            await Task.Delay(ExpectedSeconds * 1000);

            var userStatistic = await _userStatisticRepository.GetAsync(queryCache.QueryDetails);

            queryCache.IsCompleted = true;
            queryCache.Result = userStatistic.Value;

            await _cacheRepository.SetCacheAsync(
                queryCache.Id,
                queryCache);
        }
    }
}
