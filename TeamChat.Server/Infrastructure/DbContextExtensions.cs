using TeamChat.Server.Domain;
using TeamChat.Server.Infrastructure.Teams;

namespace TeamChat.Server.Infrastructure;

public static class DbContextExtensions
{
    public static void Seed(this TeamChatDbContext dbContext)
    {
        if (dbContext.Team.Any() == false)
        {
            var exampleTeam = new Team("Dev team", "Internal development team");
            var dotNetGroup = new Group(".NET");
            var angularGroup = new Group("Angular");
            exampleTeam.AddGroup(dotNetGroup);
            exampleTeam.AddGroup(angularGroup);
            dbContext.Team.Add(exampleTeam);
            dbContext.SaveChanges();
        }

        if (dbContext.User.Any() == false)
        {
            var hash = BCrypt.Net.BCrypt.EnhancedHashPassword("test");
            var user = new User("liam", "Liam", "Björkman", "liam.bjorkman@teamchat.com", hash, UserRole.Admin);
            dbContext.User.Add(user);
            dbContext.SaveChanges();

            var team = dbContext.Team.First();
            team.AddUser(user);
            dbContext.SaveChanges();
        }
    }
}
