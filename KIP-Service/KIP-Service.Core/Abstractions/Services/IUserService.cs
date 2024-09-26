using CSharpFunctionalExtensions;

namespace KIP_Service.Application.Services
{
    public interface IUserService
    {
        Task<Result<Guid>> CreateUserAsync();
        Task SingInAsync(Guid userId);
    }
}