using LanguageExt;
using LanguageExt.Common;
using TeamChat.Server.Domain;

namespace TeamChat.Server.Application.Users;
public sealed class UserService(IUserRepository repo) : IUserService
{
    public async Task<UserDto[]> GetUsers()
    {
        return (await repo.Get())
            .Map(MapUserToDto)
            .ToArray();
    }

    public async Task<Either<Error, UserDto>> GetUser(int id)
    {
        var user = await repo.GetByIdAsync(id);

        return user is not null
            ? MapUserToDto(user)
            : Error.New("User not found");
    }
    public async Task<Option<Error>> Update(UserDto userDto)
    {
        var user = await repo.GetByIdAsync(userDto.Id);

        if (user is null)
        {
            return Error.New("User not found");
        }

        user.Update(userDto.FirstName, userDto.LastName, userDto.Email, userDto.Role);

        repo.Update(user);
        await repo.SaveChangesAsync();

        return Option<Error>.None;
    }

    public async Task<Option<Error>> Delete(int id)
    {
        var user = await repo.GetByIdAsync(id);
        if (user is null)
        {
            return Error.New("User not found");
        }

        await repo.Delete(user.Id);

        return Option<Error>.None;
    }

    private static UserDto MapUserToDto(User user)
    {
        return new UserDto(user.Id, user.Username, user.FirstName, user.LastName, user.Email, user.Role);
    }
}