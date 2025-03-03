using LanguageExt;
using LanguageExt.Common;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using TeamChat.Server.Application.Auth.Interface;
using TeamChat.Server.Application.Auth.Dto;
using TeamChat.Server.Domain;
using TeamChat.Server.Infrastructure.Auth;
using TeamChat.Server.Infrastructure.Teams;

using BC = BCrypt.Net.BCrypt;

namespace TeamChat.Server.Application.Auth;
public class AuthService(ITokenService tokenService, ITeamChatDb db) : IAuthService
{
    public async Task<Either<Error, int>> RegisterUser(RegisterUserDto dto)
    {
        var userExists = await db.User.AnyAsync(x => x.Username == dto.UserName);

        if (userExists)
        {
            return Error.New("User already exists");
        }

        var hash = BC.EnhancedHashPassword(dto.Password);
        var newUser = new User(
            dto.UserName,
            dto.FirstName,
            dto.LastName,
            dto.Email,
            hash,
            dto.Role);

        db.User.Add(newUser);
        await db.SaveChangesAsync();

        return newUser.Id;
    }
    public async Task<Either<Error, UserTokenDto>> LoginUser(LoginUserDto dto)
    {
        var user = await db.User.FirstOrDefaultAsync(x => x.Username == dto.Username);

        if (user is null || !BC.EnhancedVerify(dto.Password, user.Password))
        {
            return Error.New("Invalid username or password");
        }

        var token = tokenService.GenerateAccessToken(GenerateClaims(user));
        var refreshToken = tokenService.GenerateRefreshToken();

        return new UserTokenDto(token, refreshToken);
    }

    private static Claim[] GenerateClaims(User user)
    {
        var claims = new Claim[]
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.GivenName, user.FirstName),
            new(ClaimTypes.Surname, user.LastName),
            new(ClaimTypes.Role, user.Role.ToString()),
        };

        return claims;
    }
}