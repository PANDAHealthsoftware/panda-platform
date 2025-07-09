using PANDA.Api.Models;
using PANDA.Domain.Enums;

namespace PANDA.Api.Services.Auth;

public interface IJwtTokenService
{
    string GenerateToken(Models.User user, RoleType role);
}