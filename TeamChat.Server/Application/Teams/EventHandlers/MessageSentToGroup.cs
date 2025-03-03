using TeamChat.Server.Domain.Events;

namespace TeamChat.Server.Application.Teams.EventHandlers;

public sealed record MessageSentToGroup(int GroupId) : IDomainEvent;