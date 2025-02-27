using TeamChat.Server.Domain;

namespace TeamChat.Server.Application.Users;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> Get(string username);
}