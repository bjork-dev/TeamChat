﻿using LanguageExt;
using Microsoft.AspNetCore.Mvc;
using TeamChat.Server.Infrastructure;

namespace TeamChat.Server.Application.Teams;

public static class TeamEndpoints
{
    public static WebApplication MapTeamEndpoints(this WebApplication app)
    {
        app.MapGet("/api/teams/team/{id:int:required}", async ([FromServices] ITeamService service, int id) =>
        {
            var teams = await service.GetAsync(id);

            return teams.Match(
                Right: Results.Ok,
                Left: error => Results.NotFound(error.Message)
            );
        })
            .RequireAuthorization("Authenticated")
            .WithName("GetTeam");

        app.MapGet("/api/teams", async ([FromServices] ITeamService service) =>
        {
            var result = await service.GetAsync();
            return Results.Ok(result);
        })
            .RequireAuthorization("Authenticated")
            .WithName("GetTeams");


        app.MapGet("/api/teams/user/{userId:int:required}", async ([FromServices] ITeamService service, int userId) =>
            {
                var result = await service.GetUserTeams(userId);
                return Results.Ok(result);
            })
            .RequireAuthorization("Authenticated")
            .WithName("GetTeamsForUser");

        app.MapPost("/api/teams", async ([FromServices] ITeamService service, TeamDto dto) =>
        {
            var result = await service.CreateAsync(dto);
            return result.Match(
                Right: team => Results.Created($"/teams/{team.Id}", team),
                Left: error => Results.BadRequest(error.Message)
            );
        })
            .RequireAuthorization("Admin")
            .WithName("CreateTeam");

        app.MapPut("/api/teams", async ([FromServices] ITeamService service, TeamDto dto) =>
        {
            var result = await service.UpdateAsync(dto);
            return result.Match(
                Some: error => Results.NotFound(error.Message),
                None: Results.NoContent
            );
        })
            .RequireAuthorization("Admin")
            .WithName("UpdateTeam");

        app.MapDelete("/api/teams/{id:int:required}", async ([FromServices] ITeamService service, int id) =>
        {
            var result = await service.DeleteAsync(id);
            return result.Match(
                Some: error => Results.NotFound(error.Message),
                None: Results.NoContent
            );
        })
            .RequireAuthorization("Admin")
            .WithName("DeleteTeam");

        app.MapGet("/api/teams/group/{id:int:required}", async ([FromServices] ITeamService service, int id) =>
            {
                var result = await service.GetGroupDetails(id);
                return result.Match(
                    Right: Results.Ok,
                    Left: error => Results.NotFound(error.Message));
            })
        .RequireAuthorization("Authenticated")
        .WithName("GetGroupDetails");

        app.MapPost("/api/teams/group/{groupId:int:required}/message", async ([FromServices] ITeamService service,
                HttpContext ctx,
                int groupId,
                [FromBody] SendMessageDto message) =>
            {
                var nameIdentifier = ctx.User.Claims.FirstOrDefault(x => x.Type.EndsWith("nameidentifier"))?.Value;
                if (int.TryParse(nameIdentifier, out var userId) == false)
                {
                    return Results.BadRequest("Missing userId");
                }

                var result = await service.AddMessageToGroup(groupId, userId, message.Content);

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

internal sealed record SendMessageDto(string Content);
