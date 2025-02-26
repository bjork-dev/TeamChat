namespace TeamChat.Server.Domain;

public class UserTeam
{
    public int UserId { get; private set; }
    public User User { get; private set; } = null!;
    public int TeamId { get; private set; }
    public Team Team { get; private set; } = null!;
}
