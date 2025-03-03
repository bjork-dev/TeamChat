using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
// ReSharper disable UnusedMember.Global

namespace TeamChat.Server.Application;
[Authorize(Roles = "User, Admin")]
public sealed class TeamChatHub : Hub
{
    public Task JoinGroup(string groupId)
    {
        return Groups.AddToGroupAsync(Context.ConnectionId, groupId);
    }
    public Task LeaveGroup(string groupId)
    {
        return Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId);
    }
}
