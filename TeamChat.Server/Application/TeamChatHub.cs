using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Microsoft.AspNetCore.SignalR;

namespace TeamChat.Server.Application;
[Authorize(Roles = "User, Admin")]
public sealed class TeamChatHub(ILogger<TeamChatHub> logger) : Hub
{
    //public override Task OnConnectedAsync()
    //{
    //    if (Context.GetHttpContext()!.Request.Query.TryGetValue("groupId", out var groupId))
    //    {
    //        Groups.AddToGroupAsync(Context.ConnectionId, groupId.First()!);
    //    }
    //    else
    //    {
    //        throw new ArgumentException("Missing group id in hub connection");
    //    }

    //    return base.OnConnectedAsync();
    //}

    //public override Task OnDisconnectedAsync(Exception? exception)
    //{
    //    if(Context.GetHttpContext()!.Request.Query.TryGetValue("groupId", out var groupId))
    //    {
    //        Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId.First()!);
    //    }

    //    return base.OnDisconnectedAsync(exception);
    //}

    public Task JoinGroup(string groupId)
    {
        return Groups.AddToGroupAsync(Context.ConnectionId, groupId);
    }
}
