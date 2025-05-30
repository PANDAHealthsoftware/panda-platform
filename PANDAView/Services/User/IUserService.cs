using PANDA.Shared.DTOs.User;

namespace PANDAView.Services.User;

public interface IUserService
{
    Task<List<UserDto>> GetAllUsersAsync();
    Task<UserDto?> GetUserByIdAsync(int id);
    Task CreateUserAsync(CreateUserDto user);
    Task UpdateUserAsync(int id, UpdateUserDto user);
    Task DeleteUserAsync(int id);
}