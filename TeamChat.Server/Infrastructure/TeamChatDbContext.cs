using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TeamChat.Server.Domain;
using TeamChat.Server.Domain.Base;

namespace TeamChat.Server.Infrastructure;

public sealed class TeamChatDbContext(DbContextOptions<TeamChatDbContext> options, IMediator mediator) : DbContext(options)
{
    public DbSet<Team> Team => Set<Team>();
    public DbSet<Group> Group => Set<Group>();
    public DbSet<User> User => Set<User>();
    public DbSet<Message> Message => Set<Message>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        var result =await base.SaveChangesAsync(cancellationToken);
        await DispatchDomainEvents();
        return result;
    }

    private async Task DispatchDomainEvents()
    {
        var domainEntities = ChangeTracker
            .Entries<BaseEntity>()
            .Where(x => x.Entity.DomainEvents.Count != 0)
            .ToArray();

        foreach (var entity in domainEntities)
        {
            var events = entity.Entity.DomainEvents?.ToArray();
            entity.Entity.ClearDomainEvents();
            if (events == null) continue;

            foreach (var domainEvent in events)
            {
                await mediator.Publish(domainEvent);
            }
        }
    }
}