using CSharpFunctionalExtensions;
using KIP_Service.Core.Models;
using KIP_Service.DataAccess.Repositories;

namespace KIP_Service.Application.Services
{
    public class UserService(
        IUserRepository userRepository,
        IUserStatisticRepository userStatisticRepository) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IUserStatisticRepository _userStatisticRepository = userStatisticRepository;

        public async Task<Result<Guid>> CreateUserAsync()
        {
            var user = new User(Guid.NewGuid());

            return await _userRepository.AddAsync(user);
        }

        public async Task SingInAsync(Guid userId)
        {
            var userSingIn = new UserSingIn(
                Guid.NewGuid(),
                userId,
                DateTime.UtcNow);

            await _userStatisticRepository.AddSinginAsync(userSingIn);
        }
    }
}
