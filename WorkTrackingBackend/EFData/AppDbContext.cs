using Entities;
using Microsoft.EntityFrameworkCore;

namespace EFData;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Administrator> Administrators { get; set; } = null!;
    public DbSet<Firm> Firms { get; set; } = null!;
    public DbSet<OwnershipSystem> OwnershipSystems { get; set; } = null!;
    public DbSet<TaxationSystem> TaxationSystems { get; set; } = null!;
    public DbSet<Unit> Units { get; set; } = null!;
    public DbSet<AccountableTask> Tasks { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}