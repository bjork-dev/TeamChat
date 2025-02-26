using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TeamChat.Server.Domain;

namespace TeamChat.Server.Infrastructure;

public sealed class TeamChatDbContext(DbContextOptions<TeamChatDbContext> options): DbContext(options)
{
    public DbSet<Team> Team => Set<Team>();
    public DbSet<Group> Group => Set<Group>();
    public DbSet<User> User => Set<User>();
    public DbSet<Message> Message => Set<Message>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}