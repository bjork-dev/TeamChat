using LanguageExt;
using LanguageExt.Common;
using System.Security.Claims;
using TeamChat.Server.Application.Auth.Interface;
using TeamChat.Server.Application.Auth.Dto;
using TeamChat.Server.Domain;

using BC = BCrypt.Net.BCrypt;
using TeamChat.Server.Application.Users;

namespace TeamChat.Server.Application.Auth;
public class AuthService(ITokenService tokenService, IUserRepository repo) : IAuthService
{
    public async Task<Either<Error, int>> RegisterUser(RegisterUserDto dto)
    {
        var userExists = await repo.Exists(dto.UserName);

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

        repo.Add(newUser);
        await repo.SaveChangesAsync();

        return newUser.Id;
    }
    public async Task<Either<Error, UserTokenDto>> LoginUser(LoginUserDto dto)
    {
        var user = await repo.Get(dto.Username);

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