using LanguageExt;
using LanguageExt.Common;

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
    public Either<Error, Unit> AddMessage(Message message)
    {
        if(string.IsNullOrEmpty(message.Text))
        {
            return Error.New("Message text cannot be empty.");
        }

        Messages.Add(message);

        return Unit.Default;
    }
    public void RemoveMessage(Message message)
    {
        Messages.Remove(message);
    }
}