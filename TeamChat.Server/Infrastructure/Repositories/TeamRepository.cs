using LanguageExt;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using TeamChat.Server.Domain;

namespace TeamChat.Server.Infrastructure.Repositories;

public sealed class TeamRepository(TeamChatDbContext dbContext) : IGenericRepository<Team>
{
    public Task<Team[]> GetAllAsync()
    {
        throw new NotImplementedException();
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

    public async Task<Option<Unit>> Delete(int id)
    {
        var team = await dbContext.Team.FindAsync(id);
        
        if (team is null)
        {
            return Option<Unit>.None;
        }

        dbContext.Team.Remove(team);

        await SaveChangesAsync();

        return Unit.Default;
    }
    public Task SaveChangesAsync()
    {
        return dbContext.SaveChangesAsync();
    }
}

public interface IGenericRepository<in T> where T : IAggregateRoot
{
    Task<Team?> GetByIdAsync(int id);
    void Add(T entity);
    void Update(T entity);
    Task<Option<Unit>> Delete(int id);
    Task SaveChangesAsync();
}
/// <summary>
/// Marker interface for aggregate root
/// </summary>
public interface IAggregateRoot;
