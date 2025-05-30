using PANDA.Api.Models;
using PANDA.Domain.Enums;
using PANDA.Shared.DTOs.User;

namespace PANDA.Api.Services.User;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto?> GetByIdAsync(int id);
    Task<RoleType?> GetUserRoleEnumAsync(int id);
    Task<Models.User?> ValidateCredentialsAsync(string username, string password);
    Task<List<Role>> GetUserRolesCollectionByUserIdAsync(int id);
    Task<IEnumerable<Role>> GetUserRolesByUserIdAsync(int id);
}