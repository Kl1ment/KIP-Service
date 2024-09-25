using CSharpFunctionalExtensions;
using KIP_Service.Core.Models;
using KIP_Service.DataAccess.Entities;
using Microsoft.Extensions.Logging;

namespace KIP_Service.DataAccess.Repositories
{
    public class UserRepository(
        KIP_ServiceDbContext context,
        ILogger<UserRepository> logger) : IUserRepository
    {
        private readonly KIP_ServiceDbContext _context = context;
        private readonly ILogger<UserRepository> _logger = logger;

        public async Task<Result<Guid>> AddAsync(User user)
        {
            try
            {
                var userEntity = new UserEntity
                {
                    Id = user.Id,
                };

                await _context.Users.AddAsync(userEntity);
                await _context.SaveChangesAsync();

                return user.Id;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                return Result.Failure<Guid>(ex.Message);
            }
        }
    }
}
