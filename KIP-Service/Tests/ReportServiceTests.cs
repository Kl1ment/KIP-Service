using KIP_Service.Application.Services;
using KIP_Service.Core.Models;
using KIP_Service.DataAccess.Repositories;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Tests
{
    public class ReportServiceTests
    {
        Mock<IUserStatisticRepository> _mockUserStatisticRepository = new();
        Mock<ICacheRepository> _mockCacheRepository = new();
        Mock<IConfiguration> _mockConfiguration = new();
        Mock<IReportService> _mockReportService = new();

        [Fact]
        public async void ExecuteGetUserStatisticAsync()
        {
            var userId = Guid.NewGuid();
            var from = new DateTime(2024, 9, 20);
            var to = new DateTime(2024, 9, 25);
            var requestStatistic = new RequestStatistic(userId, from, to);
            var userStatistic = new UserStatistic(userId, 1);
            var queryCache = new QueryCache<RequestStatistic, UserStatistic>(
                Guid.NewGuid(),
                nameof(RequestStatistic),
                DateTime.Now,
                requestStatistic);
            _mockConfiguration.Setup(c => c.GetSection("ExpectedSeconds").Value).Returns("5");
            _mockUserStatisticRepository.Setup(r => r.GetAsync(requestStatistic)).ReturnsAsync(userStatistic);
            var reportService = new ReportService(
                _mockUserStatisticRepository.Object,
                _mockCacheRepository.Object,
                _mockConfiguration.Object);


            reportService.ExecuteGetUserStatisticAsync(queryCache);


            _mockCacheRepository.Verify(c => c.SetCacheAsync(
                It.IsAny<Guid>(), It.IsAny<QueryCache<RequestStatistic, UserStatistic>>()));

            await Task.Delay(2 * 1000);
            _mockUserStatisticRepository.VerifyNoOtherCalls();

            await Task.Delay(5 * 1000);
            _mockUserStatisticRepository.Verify(r => r.GetAsync(
                It.Is<RequestStatistic>(s => s.UserId == userId && s.From == from && s.To == to)));
            _mockCacheRepository.Verify(c => c
                .SetCacheAsync(queryCache.Id, It.Is<QueryCache<RequestStatistic, UserStatistic>>(
                    q => q.IsCompleted == true && 
                         q.Result == userStatistic)));
        }
    }
}