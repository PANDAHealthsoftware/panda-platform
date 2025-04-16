using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace PANDA.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // Temporary hardcoded users (for dev only)
        var users = new List<(string Username, string Password, string Role)>
        {
            ("admin", "admin123", "Admin"),
            ("reception", "reception123", "Reception"),
            ("clinician", "clinician123", "Clinician")
        };

        var user = users.FirstOrDefault(u => u.Username == request.Username && u.Password == request.Password);
        if (user == default)
            return Unauthorized("Invalid credentials");

        // Build JWT
        var key = new SymmetricSecurityKey("super-secure-signing-key-must-be-at-least-32-chars"u8.ToArray());
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: "https://panda-api.local",
            audience: "panda-api",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return Ok(new { accessToken = new JwtSecurityTokenHandler().WriteToken(token) });
    }
}

public record LoginRequest(string Username, string Password);