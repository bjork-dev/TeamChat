using Microsoft.AspNetCore.Mvc;
using TeamChat.Server.Application.Teams.Dto;
using TeamChat.Server.Infrastructure;

namespace TeamChat.Server.Application.Teams;

public static class TeamEndpoints
{
    public static WebApplication MapTeamEndpoints(this WebApplication app)
    {
        app.MapGet("/api/teams/user", async ([FromServices] TeamService service, HttpContext ctx) =>
            {
                var userId = ctx.GetUserId();
                if (userId == null)
                {
                    return Results.BadRequest("Missing userId");
                }
                var result = await service.GetUserTeams(userId.Value);
                return Results.Ok(result);
            })
            .RequireAuthorization("Authenticated")
            .WithName("GetTeamsForUser");

        app.MapGet("/api/teams/group/{id:int:required}", async ([FromServices] TeamService service, int id) =>
            {
                var result = await service.GetGroupDetails(id);
                return result.Match(
                    Right: Results.Ok,
                    Left: error => Results.NotFound(error.Message));
            })
        .RequireAuthorization("Authenticated")
        .WithName("GetGroupDetails");

        app.MapPost("/api/teams/group/{groupId:int:required}/message", async ([FromServices] TeamService service,
                HttpContext ctx,
                int groupId,
                [FromBody] SendMessageDto message) =>
            {
                var userId = ctx.GetUserId();
                if (userId == null)
                {
                    return Results.BadRequest("Missing userId");
                }

                var result = await service.AddMessageToGroup(groupId, userId.Value, message.Content);

                return result.Match(
                    Some: error => Results.BadRequest(error.Message),
                    None: Results.Ok()
                );
            })
            .RequireAuthorization("Authenticated")
            .WithName("SendMessageToGroup");

        return app;
    }
}