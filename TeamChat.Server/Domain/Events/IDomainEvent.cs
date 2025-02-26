using MediatR;

namespace TeamChat.Server.Domain.Events;
/// <summary>
/// Something that happened in the domain that is of interest to others.
/// </summary>
public interface IDomainEvent : INotification;