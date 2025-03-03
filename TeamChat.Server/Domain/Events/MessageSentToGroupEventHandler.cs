using MediatR;
using Microsoft.AspNetCore.SignalR;
using TeamChat.Server.Application;
using TeamChat.Server.Application.Teams.EventHandlers;

namespace TeamChat.Server.Domain.Events;

internal sealed class MessageSentToGroupEventHandler(IHubContext<TeamChatHub> hubContext) : INotificationHandler<MessageSentToGroup>
{
    public async Task Handle(MessageSentToGroup notification, CancellationToken cancellationToken)
    {
        await hubContext.Clients.Group(notification.GroupId.ToString()).SendAsync("messageReceived", cancellationToken);
    }
}