using LanguageExt;
using LanguageExt.Common;
using TeamChat.Server.Domain.Base;

namespace TeamChat.Server.Domain;

public class Team : BaseEntity, IAggregateRoot
{
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public ICollection<Group> Groups { get; private set; } = [];
    public ICollection<User> Users { get; private set; } = [];

    private Team() { } // Required by EF Core

    public Team(string name, string description) : this()
    {
        Name = name;
        Description = description;
    }

    public void AddGroup(Group group)
    {
        Groups.Add(group);
    }

    public void RemoveGroup(Group group)
    {
        Groups.Remove(group);
    }

    public Option<Error> Update(string newName, string newDescription)
    {
        if (string.IsNullOrEmpty(newName))
        {
            return Error.New("Team name cannot be empty.");
        }

        if (string.IsNullOrEmpty(newDescription))
        {
            return Error.New("Team description cannot be empty.");
        }

        Name = newName;
        Description = newDescription;

        return Option<Error>.None;
    }

    public void AddMessage(Message message, Group group)
    {
        var groupInTeam = Groups.FirstOrDefault(g => g.Id == group.Id);
        if (groupInTeam is null)
        {
            throw new InvalidOperationException("Group does not belong to this team.");
        }
        var authorInTeam = Users.FirstOrDefault(u => u.Id == message.User.Id);
        if (authorInTeam is null)
        {
            throw new InvalidOperationException("Author does not belong to this team.");
        }
        
        groupInTeam.AddMessage(message);
    }
}