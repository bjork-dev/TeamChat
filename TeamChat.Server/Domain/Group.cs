using TeamChat.Server.Application.Teams.EventHandlers;
using TeamChat.Server.Domain.Base;

namespace TeamChat.Server.Domain;

public class Group : BaseEntity
{
    public string Name { get; set; } = null!;
    public ICollection<Message> Messages { get; private set; } = [];
    private Group() { } // Required by EF Core
    public Group(string name) : this()
    {
        Name = name;
    }


    public void AddMessage(Message message)
    {
        Messages.Add(message);

        AddDomainEvent(new MessageSentToGroup(Id));
    }
}