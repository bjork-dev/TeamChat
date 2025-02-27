using System.Security.Claims;

namespace TeamChat.Server.Application.Auth.Interface;

public interface ITokenService
{
    string GenerateAccessToken(Claim[] claims);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}