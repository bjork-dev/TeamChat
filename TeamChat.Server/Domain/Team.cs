namespace TeamChat.Server.Domain;

public class Team : BaseEntity
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
}