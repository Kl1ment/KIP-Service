using KIP_Service.Core.Models;
using KIP_Service.DataAccess.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Text.Json;

namespace KIP_Service.Application.Services
{
    public class TaskRefresher(
        IServiceProvider serviceProvider,
        ILogger<TaskRefresher> logger) : IHostedService
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly ILogger<TaskRefresher> _logger = logger;

        private IReportService _reportService;
        private ICacheRepository _cacheRepository;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateAsyncScope())
            {
                _reportService = scope.ServiceProvider.GetRequiredService<IReportService>();
                _cacheRepository = scope.ServiceProvider.GetRequiredService<ICacheRepository>();

                await RefreshAllNotCompletedTasks();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task RefreshAllNotCompletedTasks()
        {
            RedisKey[] keys = _cacheRepository.GetKeys();

            foreach (var key in keys)
            {
                var queryString = await _cacheRepository.GetStringAsync(key.ToString());

                QueryCache<object, object>? query = GetQuery(key, queryString);

                if (query != null && !query.IsCompleted)
                {
                    _logger.LogInformation($"Refresh query {query.QueryName} {query.Id}");

                    switch (query.QueryName)
                    {
                        case nameof(GetUserStatistic):
                            GetUserStatistic(query);
                            break;
                    }
                }
            }
        }

        private void GetUserStatistic(QueryCache<object, object> query)
        {
            RequestStatistic? requestStatistic = JsonSerializer.Deserialize<RequestStatistic>(query.QueryDetails.ToString());

            if (requestStatistic == null)
            {
                _logger.LogWarning($"Query {query.Id}. Lose data");
                return;
            }

            QueryCache<RequestStatistic, UserStatistic> newQuery = new(
                query.Id,
                query.QueryName,
                DateTime.Now,
                requestStatistic);

            _reportService.ExecuteGetUserStatisticAsync(newQuery);
        }

        private QueryCache<object, object>? GetQuery(RedisKey key, string? queryString)
        {
            if (queryString == null)
            {
                _logger.LogWarning($"Query {key} null");
                return null;
            }

            return JsonSerializer.Deserialize<QueryCache<object, object>>(queryString);
        }
    }
}
