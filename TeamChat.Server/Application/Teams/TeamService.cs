using LanguageExt;
using LanguageExt.Common;
using TeamChat.Server.Domain;

namespace TeamChat.Server.Application.Teams;

public class TeamService(IGenericRepository<Team> teamRepository) : ITeamService
{
    public async Task<TeamDto[]> GetAsync()
    {
        return (await teamRepository.Get())
            .Map(team=> new TeamDto(team.Id, team.Name, team.Description))
            .ToArray();
    }
    public async Task<Either<Error, TeamDto>> GetAsync(int id)
    {
        var team = await teamRepository.GetByIdAsync(id);
        if (team is null)
        {
            return Error.New("Team not found");
        }
        return new TeamDto(team.Id, team.Name, team.Description);
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
}