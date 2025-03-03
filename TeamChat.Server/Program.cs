using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.RateLimiting;
using TeamChat.Server.Application;
using TeamChat.Server.Application.Auth;
using TeamChat.Server.Application.Auth.Interface;
using TeamChat.Server.Application.Teams;
using TeamChat.Server.Application.Users;
using TeamChat.Server.Infrastructure;
using TeamChat.Server.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables(); // Ensures Azure App Settings override local settings

builder.Services.AddDbContext<TeamChatDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("TeamChatDb"));
});

//builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSignalR();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddMemoryCache();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Admin", policy => policy.RequireRole("Admin"))
    .AddPolicy("Authenticated", policy => policy.RequireRole("Admin", "User"));

builder.Services.AddRateLimiter(options =>
{
    // Stop the spam! Limit requests to 10 per second on all endpoints
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetTokenBucketLimiter(
            // Use IP address as a unique key for each client
            httpContext.Connection.RemoteIpAddress?.ToString() ?? throw new NullReferenceException("Missing RemoteIpAddress"),
            _ => new TokenBucketRateLimiterOptions
            {
                TokenLimit = 20, // Maximum tokens a client can accumulate
                ReplenishmentPeriod = TimeSpan.FromSeconds(5), // Replenishment interval
                TokensPerPeriod = 2, // Number of tokens added per interval
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 2 // Allow 2 extra requests to be queued
            }));

    // Set a fallback policy for requests without a defined limiter
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "TeamChatServer",
            ValidAudience = "TeamChatClient",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"] ?? throw new NullReferenceException("Missing JWT:SecretKey")))
        };
        // For SignalR token handling
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Query["access_token"];

                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(token) && path.StartsWithSegments("/hub"))
                {
                    context.Token = token;
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

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
    builder.Services.AddResponseCompression(options =>
    {
        options.EnableForHttps = true;
        options.MimeTypes = ["text/plain", "text/css", "application/javascript", "text/html", "application/json", "image/svg+xml"];
        options.Providers.Add<BrotliCompressionProvider>();
        options.Providers.Add<GzipCompressionProvider>();
    });

    builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
    {
        options.Level = System.IO.Compression.CompressionLevel.Fastest; 
    });

    builder.Services.Configure<GzipCompressionProviderOptions>(options =>
    {
        options.Level = System.IO.Compression.CompressionLevel.SmallestSize;
    });

    builder.Services.AddResponseCaching();
    builder.Services.AddSpaStaticFiles(c => c.RootPath = "wwwroot");
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

app.UseRateLimiter();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapAuthEndpoints();
app.MapUserEndpoints();
app.MapTeamEndpoints();

app.MapHub<TeamChatHub>("/hub");


if (app.Environment.IsProduction())
{
    app.UseResponseCaching();

    app.UseStaticFiles(new StaticFileOptions
    {
        OnPrepareResponse = ctx =>
        {
            var path = ctx.File.Name.ToLower();

            // Cache static assets (JS, CSS, images) for a long time
            if (path.EndsWith(".js") || path.EndsWith(".css") || path.EndsWith(".png") ||
                path.EndsWith(".jpg") || path.EndsWith(".jpeg") || path.EndsWith(".svg") ||
                path.EndsWith(".woff") || path.EndsWith(".woff2") || path.EndsWith(".ttf"))
            {
                ctx.Context.Response.Headers.CacheControl = "public, max-age=31536000, immutable";
            }
            // index.html is always fetched fresh, in case the SPA has been updated
            else if (path == "index.html")
            {
                ctx.Context.Response.Headers.CacheControl = "no-cache, no-store, must-revalidate";
                ctx.Context.Response.Headers.Pragma = "no-cache";
                ctx.Context.Response.Headers.Expires = "0";
            }
        }
    });

    app.UseSpaStaticFiles();
    app.UseSpa(s => s.Options.SourcePath = "wwwroot");
}

app.Run();
