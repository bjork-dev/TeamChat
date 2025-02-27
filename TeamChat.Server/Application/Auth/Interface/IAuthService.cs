using LanguageExt;
using LanguageExt.Common;
using TeamChat.Server.Application.Auth.Dto;

namespace TeamChat.Server.Application.Auth.Interface;

public interface IAuthService
{
    Task<Either<Error, int>> RegisterUser(RegisterUserDto dto);
    Task<Either<Error, UserTokenDto>> LoginUser(LoginUserDto dto);

}