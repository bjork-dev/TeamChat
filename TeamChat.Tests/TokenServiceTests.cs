using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using TeamChat.Server.Infrastructure.Auth;

namespace TeamChat.Tests;

public class TokenServiceTests
{
    private readonly TokenService _tokenService;
    private const string SecretKey = "supersecretkey12345supersecretkey12345supersecretkey12345supersecretkey12345supersecretkey12345";

    public TokenServiceTests()
    {
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.Setup(config => config["Jwt:SecretKey"]).Returns(SecretKey);

        _tokenService = new TokenService(mockConfiguration.Object);
    }

    [Fact]
    public void GenerateAccessToken_ReturnsValidToken()
    {
        // Arrange
        var claims = new[] { new Claim(ClaimTypes.Name, "testuser") };

        // Act
        var token = _tokenService.GenerateAccessToken(claims);

        // Assert
        Assert.NotNull(token);
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "TeamChatServer",
            ValidAudience = "TeamChatClient",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey)),
            ValidateLifetime = true
        }, out _);

        Assert.NotNull(principal);
        Assert.Equal("testuser", principal.Identity?.Name);
    }

    [Fact]
    public void GenerateRefreshToken_ReturnsValidToken()
    {
        // Act
        var refreshToken = _tokenService.GenerateRefreshToken();

        // Assert
        Assert.NotNull(refreshToken);
        var tokenBytes = Convert.FromBase64String(refreshToken);
        Assert.Equal(32, tokenBytes.Length);
    }

    [Fact]
    public void GetPrincipalFromExpiredToken_ReturnsPrincipal()
    {
        // Arrange
        var claims = new[] { new Claim(ClaimTypes.Name, "testuser") };
        var token = _tokenService.GenerateAccessToken(claims);

        // Act
        var principal = _tokenService.GetPrincipalFromExpiredToken(token);

        // Assert
        Assert.NotNull(principal);
        Assert.Equal("testuser", principal.Identity?.Name);
    }

    [Fact]
    public void GetPrincipalFromExpiredToken_ThrowsException_ForInvalidToken()
    {
        // Arrange
        var invalidToken = "invalid.token.value";

        // Act & Assert
        Assert.Throws<SecurityTokenException>(() => _tokenService.GetPrincipalFromExpiredToken(invalidToken));
    }
}