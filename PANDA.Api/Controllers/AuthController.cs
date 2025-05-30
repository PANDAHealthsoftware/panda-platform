using Microsoft.AspNetCore.Mvc;
using PANDA.Api.Services.Auth;
using PANDA.Api.Services.User;
using PANDA.Shared.DTOs.Auth;

namespace PANDA.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    
    private readonly IUserService _userService; 
    private readonly IJwtTokenService _jwtTokenService;

    public AuthController(IJwtTokenService jwtTokenService, IUserService userService)
    {
        _jwtTokenService = jwtTokenService;
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto login)
    {
        var user = await _userService.ValidateCredentialsAsync(login.Username, login.Password);
        if (user == null)
            return Unauthorized();
        
        var userRoles = await _userService.GetUserRolesCollectionByUserIdAsync(user.Id);
        var token = _jwtTokenService.GenerateToken(user, userRoles);

        return Ok(new { accessToken = token });
    }
}