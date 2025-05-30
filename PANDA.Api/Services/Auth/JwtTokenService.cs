using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PANDA.Api.Models;

namespace PANDA.Api.Services.Auth;

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;
    
    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }
    
    public string GenerateToken(Models.User user, IEnumerable<Role> roles)
    {
        ArgumentNullException.ThrowIfNull(user);
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };
        // Add roles
        claims.AddRange(from r in roles where !string.IsNullOrEmpty(r.Name) select new Claim(ClaimTypes.Role, r.Name));

        var key = _configuration["JwtSettings:Key"];
        if (string.IsNullOrEmpty(key))
            throw new InvalidOperationException("JWT signing key not configured.");

        var issuer = _configuration["JwtSettings:Issuer"];
        var audience = _configuration["JwtSettings:Audience"];

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}