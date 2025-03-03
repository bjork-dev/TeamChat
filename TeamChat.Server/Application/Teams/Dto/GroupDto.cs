namespace TeamChat.Server.Application.Teams.Dto;

public sealed record GroupDto(int Id, string Name);
public sealed record GroupDetailsDto(int Id, string Name, MessageDto[] Messages);
public sealed record MessageDto(int Id, int UserId, string FirstName, string LastName, string Text, DateTime CreatedAt);