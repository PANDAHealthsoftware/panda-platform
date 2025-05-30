using PANDA.Api.Models;
using PANDA.Domain.Enums;
using PANDA.Shared.DTOs.User;

namespace PANDA.Api.Services.User;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<UserDto> CreateUserAsync(CreateUserDto dto);
    Task<bool> UpdateUserAsync(UpdateUserDto dto);
    Task<bool> DeleteUserAsync(int id);
    Task<UserDto?> GetUserByIdAsync(int id);
    Task<RoleType?> GetUserRoleEnumAsync(int id);
    Task<Models.User?> ValidateCredentialsAsync(string username, string password);
    Task<List<Role>> GetUserRolesCollectionByUserIdAsync(int id);
    Task<IEnumerable<Role>> GetUserRolesByUserIdAsync(int id);
}