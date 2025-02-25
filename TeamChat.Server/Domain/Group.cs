namespace TeamChat.Server.Domain;

public class Group : BaseEntity
{
    public string Name { get; set; } = null!;
    public ICollection<Message> Messages { get; set; } = [];

    private Group() { } // Required by EF Core
    public Group(string name) : this()
    {
        Name = name;
    }
    public void AddMessage(Message message)
    {
        Messages.Add(message);
    }
    public void RemoveMessage(Message message)
    {
        Messages.Remove(message);
    }
}