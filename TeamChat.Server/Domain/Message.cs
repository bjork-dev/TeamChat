using TeamChat.Server.Domain.Base;

namespace TeamChat.Server.Domain;

public class Message : BaseEntity
{
    public User User { get; private set; } = null!;
    public string Text { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    private Message() { } // Required by EF Core
    public Message(User user, string text) : this()
    {
        User = user;
        Text = text;
        CreatedAt = DateTime.Now;
    }
}