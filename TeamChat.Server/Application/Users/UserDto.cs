using TeamChat.Server.Domain;

namespace TeamChat.Server.Application.Users;

public sealed record UserDto(int Id, string Username, string FirstName, string LastName, string Email, UserRole Role);