using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TeamChat.Server.Application.Auth.Interface;

namespace TeamChat.Server.Infrastructure.Auth;

public sealed class TokenService(IConfiguration configuration) : ITokenService
{
    private readonly SymmetricSecurityKey _secretKey =
        new(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"] ?? throw new NullReferenceException("Missing key for JWT")));

    public string GenerateAccessToken(Claim[] claims)
    {
        var signinCredentials = new SigningCredentials(_secretKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "TeamChatServer",
            audience: "TeamChatClient",
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: signinCredentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenString;
    }
    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();

        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidAudience = "TeamChatClient",
            ValidIssuer = "TeamChatServer",
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = _secretKey,
            ValidateLifetime = true
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        ClaimsPrincipal principal;
        try
        {
            principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }
        }
        catch
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }
}

public interface ITokenService
{
    string GenerateAccessToken(Claim[] claims);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}

