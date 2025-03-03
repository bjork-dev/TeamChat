using Microsoft.EntityFrameworkCore;
using TeamChat.Server.Application.Teams;

namespace TeamChat.Server.Infrastructure;

public static class TeamChatRepository
{
    public static Task<TeamDto[]> GetTeamsForUser(this TeamChatDbContext db, int userId)
        => db.Team
            .AsNoTracking()
            .Include(t => t.Users)
            .Where(t => t.Users.Any(u => u.Id == userId))
            .Select(t => new TeamDto(t.Id, t.Name, t.Description, t.Groups
                .Select(g => new GroupDto(g.Id, g.Name))
                .ToArray()))
            .ToArrayAsync();


    public static Task<GroupDetailsDto?> GetGroup(this TeamChatDbContext db, int id)
        => db.Group
            .AsNoTracking()
            .Include(x => x.Messages)
            .ThenInclude(x => x.User)
            .Where(x => x.Id == id)
            .Select(x => new GroupDetailsDto(
                x.Id,
                x.Name,
                x.Messages
                    .Select(m => new MessageDto(m.Id, m.User.Id, m.User.FirstName, m.User.LastName, m.Text, m.CreatedAt))
                    .ToArray()))
            .FirstOrDefaultAsync();

}