using Entities;

namespace UseCases.Repositories;

public interface IUserRepository
{
    public IQueryable<User> Users { get; }
    public void AddUser(User user);
}
