﻿using LanguageExt;
using LanguageExt.Common;

namespace TeamChat.Server.Application.Teams;

public interface ITeamService
{
    Task<Either<Error, TeamDto>> GetAsync(int id);
    Task<TeamDto[]> GetAsync();
    Task<Either<Error, TeamDto>> CreateAsync(TeamDto dto);
    Task<Option<Error>> UpdateAsync(TeamDto dto);
    Task<Option<Error>> DeleteAsync(int id);

}