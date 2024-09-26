using KIP_Service.Core.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Text.Json;

namespace KIP_Service.Application.Services
{
    public class TaskRefresher(
        IConfiguration configuration,
        IDistributedCache distributedCache,
        IServiceProvider serviceProvider,
        ILogger<TaskRefresher> logger) : IHostedService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IDistributedCache _distributedCache = distributedCache;
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly ILogger<TaskRefresher> _logger = logger;

        public async Task RefreshAllNotCompletedTasks()
        {
            var connection = await ConnectionMultiplexer.ConnectAsync(_configuration.GetConnectionString("Redis"));

            var endPoint = connection.GetEndPoints().First();

            var keys = connection.GetServer(endPoint).Keys(pattern: "*").ToArray();

            foreach (var key in keys)
            {
                var task = await _distributedCache.GetStringAsync(key.ToString());

                QueryCache<RequestStatistic, UserStatistic>? query = null;

                if (task != null)
                    query = JsonSerializer.Deserialize<QueryCache<RequestStatistic, UserStatistic>>(task);

                if (query != null && !query.IsCompleted)
                {
                    _logger.LogInformation($"Refresh query {query.Id}");

                    query.CreatedDate = DateTime.Now;

                    using (var scope = _serviceProvider.CreateAsyncScope())
                    {
                        var reportService = scope.ServiceProvider.GetRequiredService<IReportService>();

                        reportService.ExecuteGetUserStatisticAsync(query);
                    }
                }
            }
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await RefreshAllNotCompletedTasks();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
