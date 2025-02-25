namespace TeamChat.Server.Domain;

public class User : BaseEntity
{
    public string Name { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string Password { get; private set; } = null!;
    public UserRole Role { get; private set; }
    public ICollection<Team> Teams { get; private set; } = [];

    private User() { } // Required by EF Core
    public User(string name, string email, string password, UserRole role) : this()
    {
        Name = name;
        Email = email;
        Password = password;
        Role = role;
    }

    public void ChangeRole(UserRole role)
    {
        Role = role;
    }

    public void ChangePassword(string password)
    {
        Password = password;
    }

    public void AddTeam(Team team)
    {
        Teams.Add(team);
    }

    public void RemoveTeam(Team team)
    {
        Teams.Remove(team);
    }
}