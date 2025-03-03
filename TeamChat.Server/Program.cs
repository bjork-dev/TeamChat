using Microsoft.EntityFrameworkCore;
using TeamChat.Server.Application;
using TeamChat.Server.Application.Auth;
using TeamChat.Server.Application.Auth.Interface;
using TeamChat.Server.Application.Teams;
using TeamChat.Server.Infrastructure;
using TeamChat.Server.Infrastructure.Auth;
using TeamChat.Server.Infrastructure.Teams;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables(); // Ensures Azure App Settings override local settings

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSignalR();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddMemoryCache();

builder.Services.AddDbContext<TeamChatDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("TeamChatDb"));
});

builder.Services.AddScoped<ITeamChatDb>(sp => sp.GetRequiredService<TeamChatDbContext>());
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<TeamService>();

builder.AddAuthenticationAndAuthorization();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
    {
        // Enable CORS in development environment to allow requests from Angular dev server
        // Not needed in production as Angular app is served from the same server
        options.AddPolicy("AllowAll", p => p
            .WithOrigins("http://localhost:4200")
            .AllowCredentials()
            .AllowAnyMethod()
            .AllowAnyHeader());
    });
}

if (builder.Environment.IsProduction())
{
    builder.AddSpaServer();
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    using var scope = app.Services.CreateScope();
    using var dbContext = scope.ServiceProvider.GetRequiredService<TeamChatDbContext>();

    dbContext.Database.Migrate();

    dbContext.Seed();

    app.UseCors("AllowAll");
}

if (app.Environment.IsProduction())
{
    app.UseRateLimiter();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapAuthEndpoints();
app.MapTeamEndpoints();

app.MapHub<TeamChatHub>("/hub");

if (app.Environment.IsProduction())
{
    app.UseSpaServer();
}

app.Run();
