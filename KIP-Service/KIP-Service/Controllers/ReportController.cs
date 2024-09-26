using KIP_Service.Application.Services;
using KIP_Service.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace KIP_Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportController(IReportService reportService) : ControllerBase
    {
        private readonly IReportService _reportService = reportService;

        [HttpPost("user_statistics")]
        public ActionResult<Guid> GetUserStatistic(UserStatisticRequest userStatisticRequest)
        {
            return _reportService.GetUserStatistic(
                userStatisticRequest.UserId,
                userStatisticRequest.From,
                userStatisticRequest.To);
        }

        [HttpGet("info")]
        public async Task<ActionResult<QueryInfoResponse<UserStatisticResponse>>> GetQueryInfo(Guid queryId)
        {
            var result = await _reportService.GetQueryInfoAsync(queryId);

            if (result.IsFailure)
                return BadRequest(result.Error);

            UserStatisticResponse? userStatisticResponse = null;
            
            if (result.Value.Result != null)
            {
                userStatisticResponse = new(
                    result.Value.Result.Id,
                    result.Value.Result.CountSignIn);
            }

            return new QueryInfoResponse<UserStatisticResponse>(
                result.Value.Id,
                result.Value.Percent,
                userStatisticResponse);
        }
    }
}
