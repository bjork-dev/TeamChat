using LanguageExt;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using TeamChat.Server.Application;
using TeamChat.Server.Domain;

namespace TeamChat.Server.Infrastructure.Repositories;

public sealed class TeamRepository(TeamChatDbContext dbContext) : IGenericRepository<Team>
{
    public Task<Team[]> Get()
    {
        return dbContext.Team
            .AsNoTracking()
            .ToArrayAsync();
    }

    public Task<Team?> GetByIdAsync(int id)
    {
        return dbContext.Team
            .AsNoTracking()
            .Include(t => t.Groups)
            .Include(t => t.Users)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public void Add(Team entity)
    {
        dbContext.Team.Add(entity);
    }

    public void Update(Team entity)
    {
        dbContext.Team.Update(entity);
    }

    public async Task<Option<Error>> Delete(int id)
    {
        var team = await dbContext.Team.FindAsync(id);
        
        if (team is null)
        {
            return Error.New("Team not found");
        }

        dbContext.Team.Remove(team);

        await SaveChangesAsync();

        return Option<Error>.None;
    }

    public Task<bool> Exists(string name)
    {
        return dbContext.Team.AnyAsync(x => x.Name == name);
    }

    public Task SaveChangesAsync()
    {
        return dbContext.SaveChangesAsync();
    }
}