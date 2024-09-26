using KIP_Service.Application.Services;
using KIP_Service.Core.Models;
using KIP_Service.DataAccess.Repositories;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Tests
{
    public class ReportServiceTests
    {
        private Mock<IUserStatisticRepository> _mockUserStatisticRepository = new();
        private Mock<ICacheRepository> _mockCacheRepository = new();
        private Mock<IConfiguration> _mockConfiguration = new();

        [Fact]
        public async void ExecuteGetUserStatisticAsync()
        {
            QueryCache<RequestStatistic, UserStatistic> queryCache = GetQueryCache();
            var userStatistic = new UserStatistic(queryCache.QueryDetails.UserId, 1);
            _mockConfiguration.Setup(c => c.GetSection("ExpectedSeconds").Value).Returns("5");
            _mockUserStatisticRepository.Setup(r => r.GetAsync(queryCache.QueryDetails)).ReturnsAsync(userStatistic);
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
            _mockUserStatisticRepository.Verify(r => r.GetAsync(queryCache.QueryDetails));
            _mockCacheRepository.Verify(c => c
                .SetCacheAsync(queryCache.Id, It.Is<QueryCache<RequestStatistic, UserStatistic>>(
                    q => q.IsCompleted == true &&
                         q.Result == userStatistic)));
        }

        [Fact]
        public async void GetQueryInfoAsync_ResultReturnQueryInfo()
        {
            var queryCache = GetQueryCache();
            _mockCacheRepository.Setup(c => c.GetAsync<RequestStatistic, UserStatistic>(queryCache.Id))
                .ReturnsAsync(queryCache);
            _mockConfiguration.Setup(c => c.GetSection("ExpectedSeconds").Value).Returns("5");
            var reportService = new ReportService(
                _mockUserStatisticRepository.Object,
                _mockCacheRepository.Object,
                _mockConfiguration.Object);

            var result = await reportService.GetQueryInfoAsync(queryCache.Id);

            Assert.IsType<QueryInfo<UserStatistic>>(result.Value);
            Assert.Equal(queryCache.Id, result.Value.Id);
            Assert.Equal(queryCache.Result, result.Value.Result);
        }


        [Fact]
        public async void GetQueryInfoAsync_ResultIsFailure()
        {
            var userId = Guid.NewGuid();
            QueryCache<RequestStatistic, UserStatistic>? queryCache = null;
            _mockCacheRepository.Setup(c => c.GetAsync<RequestStatistic, UserStatistic>(userId))
                .ReturnsAsync(queryCache);
            _mockConfiguration.Setup(c => c.GetSection("ExpectedSeconds").Value).Returns("5");
            var reportService = new ReportService(
                _mockUserStatisticRepository.Object,
                _mockCacheRepository.Object,
                _mockConfiguration.Object);

            var result = await reportService.GetQueryInfoAsync(userId);

            Assert.True(result.IsFailure);
            Assert.Equal("Query has not been found", result.Error);
        }

        private QueryCache<RequestStatistic, UserStatistic> GetQueryCache()
        {
            var from = new DateTime(2024, 9, 20);
            var to = new DateTime(2024, 9, 25);
            var requestStatistic = new RequestStatistic(Guid.NewGuid(), from, to);
            
            return new QueryCache<RequestStatistic, UserStatistic>(
                            Guid.NewGuid(),
                            nameof(RequestStatistic),
                            DateTime.Now,
                            requestStatistic);
        }
    }
}