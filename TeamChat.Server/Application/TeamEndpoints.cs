using LanguageExt;
using Microsoft.AspNetCore.Mvc;

namespace TeamChat.Server.Application;

public static class TeamEndpoints
{
    public static WebApplication MapTeamEndpoints(this WebApplication app)
    {
        app.MapGet("/teams/{id:int:required}", async ([FromServices] ITeamService service, int id) =>
        {
            var teams = await service.GetAsync(id);

            return teams.Match(
                Right: Results.Ok,
                Left: error => Results.NotFound(error.Message)
            );
        }).WithName("GetTeams");

        app.MapPost("/teams", async ([FromServices] ITeamService service, TeamDto dto) =>
        {
            var result = await service.CreateAsync(dto);
            return result.Match(
                Right: team => Results.Created($"/teams/{team.Id}", team),
                Left: error => Results.BadRequest(error.Message)
            );
        }).WithName("CreateTeam");

        app.MapPut("/teams", async ([FromServices] ITeamService service, TeamDto dto) =>
        {
            var result = await service.UpdateAsync(dto);
            return result.Match(
                Some: error => Results.NotFound(error.Message),
                None: Results.NoContent
            );
        }).WithName("UpdateTeam");

        app.MapDelete("/teams/{id:int:required}", async ([FromServices] ITeamService service, int id) =>
        {
            var result = await service.DeleteAsync(id);
            return result.Match(
                Some: error => Results.NotFound(error.Message),
                None: Results.NoContent
            );
        }).WithName("DeleteTeam");

        return app;
    }
}
