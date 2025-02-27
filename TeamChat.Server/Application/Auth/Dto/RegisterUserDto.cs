using TeamChat.Server.Domain;

namespace TeamChat.Server.Application.Auth.Dto;

public sealed record RegisterUserDto(
    string UserName,
    string FirstName,
    string LastName,
    string Email,
    string Password,
    UserRole Role);