using CSharpFunctionalExtensions;
using KIP_Service.Application.Services;
using KIP_Service.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests
{
    public class UserControllerTests
    {
        private Mock<IUserService> _mockUserService = new();

        [Fact]
        public async void CreateUser_ResultCallUserServiceCreateUserAsyncAndReturnUserId()
        {
            var userId = Guid.NewGuid();
            _mockUserService.Setup(s => s.CreateUserAsync()).ReturnsAsync(userId);
            var controller = new UserController(_mockUserService.Object);

            var result = await controller.CreateUser();

            Assert.IsType<ActionResult<Guid>>(result);
            Assert.Equal(userId, result.Value);
            _mockUserService.Verify(s => s.CreateUserAsync());
        }

        [Fact]
        public async void SingIn_ResultCallUserServiceSingInAsyncAndReturnOkResult()
        {
            var userId = Guid.NewGuid();
            _mockUserService.Setup(s => s.SingInAsync(userId));
            var controller = new UserController(_mockUserService.Object);

            var result = await controller.SingIn(userId);

            Assert.IsType<OkResult>(result);
            _mockUserService.Verify(s => s.SingInAsync(userId));
        }
    }
}
