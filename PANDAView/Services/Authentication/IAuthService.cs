using PANDAView.Models;

namespace PANDAView.Services.Authentication;

public interface IAuthService
{
    Task<bool> LoginAsync(LoginModel credentials);
    Task LogoutAsync();
    Task<string?> GetTokenAsync();
    Task<bool> IsLoggedInAsync();
    Task<string?> GetCurrentUserRoleAsync();
    Task<List<string>> GetCurrentUserRolesAsync();
}