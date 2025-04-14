using FluentAssertions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PANDA.Api.Tests.Helpers;

public class JwtTestHelperTests
{
    [Fact]
    public void GenerateTestToken_ShouldContainCorrectRole()
    {
        // Act
        var token = JwtTestHelper.GenerateTestToken("Clinician");

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        // Assert
        jwt.Claims.Should().ContainSingle(c => c.Type == ClaimTypes.Role && c.Value == "Clinician");
        jwt.Issuer.Should().Be("https://panda-api.local");
        jwt.Audiences.Should().Contain("panda-api");
    }
}