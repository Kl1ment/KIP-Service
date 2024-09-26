using CSharpFunctionalExtensions;
using KIP_Service.Application.Services;
using KIP_Service.Contracts;
using KIP_Service.Controllers;
using KIP_Service.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests
{
    public class ReportControllerTests()
    {
        private Mock<IReportService> _mockReportService = new();

        [Fact]
        public void GetUserStatistic_ResultCallReportServiceGetUserStatisticAndReturnQueryId()
        {
            var userStatisticRequest = new UserStatisticRequest(
                Guid.NewGuid(),
                new DateTime(2024, 9, 20),
                new DateTime(2024, 9, 25));
            var queryId = Guid.NewGuid();
            _mockReportService.Setup(s => s.GetUserStatistic(
                userStatisticRequest.UserId,
                userStatisticRequest.From,
                userStatisticRequest.To)).Returns(queryId);
            var controller = new ReportController(_mockReportService.Object);

            var result = controller.GetUserStatistic(userStatisticRequest);

            Assert.IsType<ActionResult<Guid>>(result);
            Assert.Equal(queryId, result.Value);
            _mockReportService.Verify(s => s.GetUserStatistic(
                userStatisticRequest.UserId,
                userStatisticRequest.From,
                userStatisticRequest.To));
        }

        [Fact]
        public async void GetQueryInfo_ResultCallReportServiceGetQueryInfoAsyncAndReturnQueryInfo()
        {
            var userStatistic = new UserStatistic(Guid.NewGuid(), 1);
            var queryInfo = new QueryInfo<UserStatistic>(Guid.NewGuid(), 1, userStatistic);
            _mockReportService.Setup(s => s.GetQueryInfoAsync(queryInfo.Id)).ReturnsAsync(queryInfo);
            var controller = new ReportController(_mockReportService.Object);

            var result = await controller.GetQueryInfo(queryInfo.Id);

            Assert.Equal(queryInfo.Id, result.Value?.Query);
            Assert.Equal(queryInfo.Percent, result.Value?.Percent);
            Assert.Equal(queryInfo.Result?.Id, result.Value?.result?.UserId);
            Assert.Equal(queryInfo.Result?.CountSignIn, result.Value?.result?.CountSingIn);
            _mockReportService.Verify(s => s.GetQueryInfoAsync(queryInfo.Id));
        }
    }
}