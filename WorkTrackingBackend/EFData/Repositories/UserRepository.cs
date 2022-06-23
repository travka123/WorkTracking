using Entities;
using Microsoft.EntityFrameworkCore;
using UseCases.Repositories;

namespace EFData.Repositories;

public class UserRepository : IUserRepository
{
    private AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public IQueryable<User> Users => _context.Users.AsNoTracking();

    public void AddUser(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
        _context.Entry(user).State = EntityState.Detached;
    }
}
