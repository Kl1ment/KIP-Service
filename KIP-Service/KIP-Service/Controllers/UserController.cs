using KIP_Service.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace KIP_Service.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController(
        IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateUser()
        {
            var result = await _userService.CreateUserAsync();

            if (result.IsFailure)
                return BadRequest();

            return result.Value;
        }

        [HttpPost]
        public async Task<ActionResult> SingIn(Guid userId)
        {
            await _userService.SingInAsync(userId);

            return Ok();
        }
    }
}
