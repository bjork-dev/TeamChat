using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;
using TeamChat.Server.Application.Auth.Dto;
using TeamChat.Server.Application.Auth.Interface;

namespace TeamChat.Server.Application.Auth;

public static class AuthEndpoints
{
    public static WebApplication MapAuthEndpoints(this WebApplication app)
    {
        app.MapPost("/api/login", async ([FromBody] LoginUserDto dto, [FromServices] IAuthService service) =>
        {
            var result = await service.LoginUser(dto);

            return result.Match(
                Results.Ok,
                error => Results.BadRequest(error.Message));
        });


        app.MapPost("/api/register", async ([FromBody] RegisterUserDto dto, [FromServices] IAuthService service) =>
        {
            var result = await service.RegisterUser(dto);

            return result.Match(
                Right: userId => Results.Created(),
                Left: error => Results.BadRequest(error.Message)
            );

        });

        return app;
    }

}