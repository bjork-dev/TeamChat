using LanguageExt;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TeamChat.Server.Application.Teams.Dto;
using TeamChat.Server.Domain;
using TeamChat.Server.Infrastructure.Teams;

namespace TeamChat.Server.Application.Teams;

public class TeamService(ITeamChatDb db, IMemoryCache cache)
{
    public async Task<TeamDto[]> GetUserTeams(int userId)
    {
        if (cache.TryGetValue($"{userId}-teams", out TeamDto[]? cachedTeams) && cachedTeams != null)
        {
            return cachedTeams;
        }

        var teams = await db.GetTeamsForUser(userId);

        cache.Set($"{userId}-teams", teams, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) });

        return teams;
    }
    public async Task<Either<Error, GroupDetailsDto>> GetGroupDetails(int id)
    {
        var group = await db.GetGroup(id);

        if (group == null)
        {
            return Error.New("Group does not exist");
        }

        return group;
    }

    public async Task<Option<Error>> AddMessageToGroup(int groupId, int userId, string text)
    {
        var group = await db.Group.FirstOrDefaultAsync(x => x.Id == groupId);

        if (group == null)
        {
            return Error.New("Group does not exist");
        }

        var user = await db.User.FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null)
        {
            return Error.New("User does not exist");
        }

        group.AddMessage(new Message(user, text));
        await db.SaveChangesAsync();
        return Option<Error>.None;
    }
}