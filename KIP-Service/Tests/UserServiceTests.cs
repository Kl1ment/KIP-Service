using KIP_Service.Application.Services;
using KIP_Service.Core.Models;
using KIP_Service.DataAccess.Repositories;
using Moq;

namespace Tests
{
    public class UserServiceTests
    {
        private Mock<IUserRepository> _mockUserRepository = new();
        private Mock<IUserStatisticRepository> _mockUserStatisticRepository = new();

        [Fact]
        public async void CreateUserAsync_ResultCallUserRepositoryAddAsyncAndReturnTypeGuid()
        {
            var userService = new UserService(_mockUserRepository.Object, _mockUserStatisticRepository.Object);

            var result = await userService.CreateUserAsync();

            Assert.IsType<Guid>(result.Value);
            _mockUserRepository.Verify(r => r.AddAsync(It.IsAny<User>()));
        }

        [Fact]
        public async void SingInAsync_ResultCallUserRepositoryAddSingInAsync()
        {
            var userId = Guid.NewGuid();
            var userService = new UserService(_mockUserRepository.Object, _mockUserStatisticRepository.Object);

            await userService.SingInAsync(userId);

            _mockUserStatisticRepository.Verify(r => r.AddSinginAsync(It.IsAny<UserSingIn>()));
            _mockUserStatisticRepository.Verify(r => r.AddSinginAsync(It.Is<UserSingIn>(m => m.UserId == userId)));
        }
    }
}