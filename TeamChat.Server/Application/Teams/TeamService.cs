using LanguageExt;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using TeamChat.Server.Domain;
using TeamChat.Server.Infrastructure;
using TeamChat.Server.Infrastructure.Repositories;

namespace TeamChat.Server.Application.Teams;

public class TeamService(ITeamRepository teamRepository, TeamChatDbContext dbContext) : ITeamService
{
    public async Task<TeamDto[]> GetAsync()
    {
        return (await teamRepository.Get())
            .Map(t => new TeamDto(t.Id, t.Name, t.Description, t.Groups
                .Map(g => new GroupDto(g.Id, g.Name))
                .ToArray()))
            .ToArray();
    }

    public async Task<TeamDto[]> GetUserTeams(int userId)
    {
        return (await teamRepository.GetTeamsForUser(userId))
            .Map(t => new TeamDto(t.Id, t.Name, t.Description, t.Groups
                .Map(g => new GroupDto(g.Id, g.Name))
                .ToArray()))
            .ToArray();
    }

    public async Task<Either<Error, TeamDto>> GetAsync(int id)
    {
        var team = await teamRepository.GetByIdAsync(id);
        if (team is null)
        {
            return Error.New("Team not found");
        }
        return new TeamDto(team.Id, team.Name, team.Description, team.Groups.Map(g => new GroupDto(g.Id, g.Name)).ToArray());
    }

    public async Task<Either<Error, TeamDto>> CreateAsync(TeamDto dto)
    {
        var newTeam = new Team(dto.Name, dto.Description);
        teamRepository.Add(newTeam);
        await teamRepository.SaveChangesAsync();

        return dto with { Id = newTeam.Id };
    }

    public async Task<Option<Error>> UpdateAsync(TeamDto dto)
    {
        var team = await teamRepository.GetByIdAsync(dto.Id);

        if (team is null)
        {
            return Error.New("Team not found");
        }

        var result = team.Update(dto.Name, dto.Description);

        return await result.MatchAsync(
            Some: Option<Error>.Some,
            None: async () =>
            {
                teamRepository.Update(team);
                await teamRepository.SaveChangesAsync();
                return Option<Error>.None;
            }
        );
    }

    public async Task<Option<Error>> DeleteAsync(int id)
    {
        var result = await teamRepository.Delete(id);

        return result.Match(
            Some: _ => Option<Error>.None,
            None: () => Option<Error>.Some(Error.New("Team not found"))
        );
    }

    public async Task<Either<Error, GroupDetailsDto>> GetGroupDetails(int id)
    {
        var group = await dbContext.Group
            .AsNoTracking()
            .Include(x => x.Messages)
            .ThenInclude(x => x.User)
            .Where(x => x.Id == id)
            .Select(x => new GroupDetailsDto(
                x.Id,
                x.Name,
                x.Messages
                    .Select(m => new MessageDto(m.Id, m.User.Id, m.User.FirstName, m.User.LastName, m.Text, m.CreatedAt))
                .ToArray())
            {
            })
            .FirstOrDefaultAsync();

        if (group == null)
        {
            return Error.New("Group does not exist");
        }

        return group;
    }

    public async Task<Option<Error>> AddMessageToGroup(int groupId, int userId, string text)
    {
        var group = await dbContext.Group.FirstOrDefaultAsync(x => x.Id == groupId);

        if (group == null)
        {
            return Error.New("Group does not exist");
        }

        var user = await dbContext.User.FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null)
        {
            return Error.New("User does not exist");
        }

        group.AddMessage(new Message(user, text));
        await dbContext.SaveChangesAsync();
        return Option<Error>.None;
    }
}