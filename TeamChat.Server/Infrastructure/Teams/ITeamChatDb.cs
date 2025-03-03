using Microsoft.EntityFrameworkCore;
using TeamChat.Server.Domain;

namespace TeamChat.Server.Infrastructure.Teams;

public interface ITeamChatDb
{
    DbSet<Team> Team { get; }
    DbSet<Group> Group { get; }
    DbSet<User> User { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new());
}