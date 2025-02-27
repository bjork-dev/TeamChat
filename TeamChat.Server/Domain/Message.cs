using TeamChat.Server.Domain.Base;

namespace TeamChat.Server.Domain;

public class Message : BaseEntity
{
    public User User { get; set; } = null!;
    public string Text { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    private Message() { } // Required by EF Core
    public Message(User user, string text) : this()
    {
        User = user;
        Text = text;
        CreatedAt = DateTime.Now;
    }
}