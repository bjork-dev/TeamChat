using LanguageExt;
using LanguageExt.Common;
using TeamChat.Server.Domain.Base;

namespace TeamChat.Server.Application;

public interface IGenericRepository<T> where T : IAggregateRoot
{
    Task<T[]> Get();
    Task<T?> GetByIdAsync(int id);
    void Add(T entity);
    void Update(T entity);
    Task<Option<Error>> Delete(int id);
    Task<bool> Exists(string name);
    Task SaveChangesAsync();
}