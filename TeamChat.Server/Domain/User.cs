using TeamChat.Server.Domain.Base;

namespace TeamChat.Server.Domain;

public class User : BaseEntity, IAggregateRoot
{
    public string Username { get; private set; } = null!;
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string Password { get; private set; } = null!;
    public UserRole Role { get; private set; }
    public ICollection<Team> Teams { get; private set; } = [];

    private User() { } // Required by EF Core
    public User(string username, string firstName, string lastName, string email, string password, UserRole role) : this()
    {
        Username = username;
        FirstName = firstName;
        LastName = lastName;
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

    public void Update(string firstName, string lastName, string email, UserRole role)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Role = role;
    }
}