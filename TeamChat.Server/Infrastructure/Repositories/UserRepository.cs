using LanguageExt;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using TeamChat.Server.Application.Users;
using TeamChat.Server.Domain;

namespace TeamChat.Server.Infrastructure.Repositories;
public class UserRepository(TeamChatDbContext dbContext) : IUserRepository
{
    public Task<User[]> Get()
    {
        return dbContext.User.ToArrayAsync();
    }

    public Task<User?> GetByIdAsync(int id)
    {
        return dbContext.User.FirstOrDefaultAsync(x => x.Id == id);
    }

    public void Add(User entity)
    {
        dbContext.User.Add(entity);
    }

    public void Update(User entity)
    {
        dbContext.User.Update(entity);
    }

    public async Task<Option<Error>> Delete(int id)
    {
        var user = await dbContext.User.FirstOrDefaultAsync(x => x.Id == id);
        if (user == null)
        {
            return Error.New("User not found");
        }

        dbContext.User.Remove(user);

        return Option<Error>.None;
    }

    public Task<bool> Exists(string username)
    {
        return dbContext.User.AnyAsync(x => x.Username == username);
    }

    public Task SaveChangesAsync()
    {
        return dbContext.SaveChangesAsync();
    }

    public Task<User?> Get(string username)
    {
        return dbContext.User.FirstOrDefaultAsync(x => x.Username == username);
    }
}
