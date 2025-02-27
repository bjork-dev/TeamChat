using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TeamChat.Server.Application.Users;

public static class UserEndpoints
{
    public static WebApplication MapUserEndpoints(this WebApplication app)
    {
        app.MapGet("/api/users", async ([FromServices] IUserService service) => await service.GetUsers())
            .RequireAuthorization("Admin");

        app.MapGet("/api/users/{id:int:required}", async ([FromRoute] int id, [FromServices] IUserService service) =>
        {
            var result = await service.GetUser(id);

            return result.Match(
                Results.Ok,
                error => Results.NotFound(error.Message));

        }).RequireAuthorization("Admin");

        app.MapPut("/api/users", async ([FromBody] UserDto dto, [FromServices] IUserService service) =>
        {
            var result = await service.Update(dto);

            return result.Match(
                Some: error => Results.BadRequest(error.Message),
                None: Results.NoContent
            );
        }).RequireAuthorization("Admin");

        app.MapDelete("/api/users/{id:int:required}", async ([FromRoute] int id, [FromServices] IUserService service) =>
        {
            await service.Delete(id);
            return Results.NoContent();
        }).RequireAuthorization("Admin");

        return app;
    }
}