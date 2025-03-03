namespace TeamChat.Server.Application.Teams.Dto;

public sealed record TeamDto(int Id, string Name, string Description, GroupDto[] Groups);