using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using TeamChat.Server.Application.Teams;
using TeamChat.Server.Application.Teams.Dto;
using TeamChat.Server.Domain;
using TeamChat.Server.Infrastructure.Teams;

namespace TeamChat.Tests;

public class TeamServiceTests
{
    private readonly TeamChatDbContext _context;
    private readonly IMemoryCache _cache;
    public TeamServiceTests()
    {
        var mediatr = new Mock<IMediator>();
        var options = new DbContextOptionsBuilder<TeamChatDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        _context = new TeamChatDbContext(options, mediatr.Object);
        var team = new Team("Dev team", "Internal development team");
        var user = new User("liam", "Liam", "Björkman", "email", "password", UserRole.Admin);
        team.AddUser(user);
        team.AddUser(user);
        _context.Team.Add(team);
        _context.SaveChanges();
    }

    [Fact]
    public async Task GetUserTeams_ReturnsUserTeams_AndAddedToCache()
    {
        // Arrange
        var expectedTeamDto = new TeamDto(1, "Dev team", "Internal development team", []);
        var expectedTeamDto2 = new TeamDto(2, "Dev team", "Internal development team", []);
        var cache = MockMemoryCacheService.GetMemoryCache();
        var teamService = new TeamService(_context, cache);
        cache.MockSet<TeamDto[]>("1-teams", [expectedTeamDto, expectedTeamDto2]);
        // Act
        var team = await teamService.GetUserTeams(1);
        // Assert
        Assert.NotNull(team);
        Assert.Equal("Dev team", team.First().Name);
        Assert.Equal("Internal development team", team.First().Description);


        cache.TryGetValue("1-teams", out var cachedTeams);

        Assert.NotNull(cachedTeams);

        //Assert.Equal("Dev team", cachedTeams.First().Name);
        //Assert.Equal("Internal development team", cachedTeams.First().Description);


    }
}