using MediatR;
using Microsoft.AspNetCore.SignalR;
using TeamChat.Server.Domain.Events;

namespace TeamChat.Server.Application;

public sealed record MessageSentToGroup(int GroupId) : IDomainEvent;

internal sealed class MessageSentToGroupEventHandler(IHubContext<TeamChatHub> hubContext) : INotificationHandler<MessageSentToGroup>
{
    public async Task Handle(MessageSentToGroup notification, CancellationToken cancellationToken)
    {
        await hubContext.Clients.Group(notification.GroupId.ToString()).SendAsync("messageReceived", cancellationToken);
    }
}