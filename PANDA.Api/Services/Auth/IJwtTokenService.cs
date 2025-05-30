using PANDA.Api.Models;

namespace PANDA.Api.Services.Auth;

public interface IJwtTokenService
{
    string GenerateToken(Models.User user, IEnumerable<Role> roles);
}