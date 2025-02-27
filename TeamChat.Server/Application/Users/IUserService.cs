using LanguageExt;
using LanguageExt.Common;

namespace TeamChat.Server.Application.Users;

public interface IUserService
{
    Task<UserDto[]> GetUsers();
    Task<Either<Error, UserDto>> GetUser(int id);
    Task<Option<Error>> Update(UserDto userDto);
    Task<Option<Error>> Delete(int id);
}