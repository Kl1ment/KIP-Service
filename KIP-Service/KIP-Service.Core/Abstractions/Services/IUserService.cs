using CSharpFunctionalExtensions;
using KIP_Service.Core.Models;

namespace KIP_Service.Application.Services
{
    public interface IUserService
    {
        Task<Result<Guid>> CreateUserAsync();
        Task SingInAsync(Guid userId);
    }
}