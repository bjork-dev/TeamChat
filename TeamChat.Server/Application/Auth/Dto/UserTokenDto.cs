namespace TeamChat.Server.Application.Auth.Dto;

public sealed record UserTokenDto(string Token, string RefreshToken);