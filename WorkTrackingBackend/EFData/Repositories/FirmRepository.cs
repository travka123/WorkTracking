using Entities;
using Microsoft.EntityFrameworkCore;
using UseCases.Repositories;

namespace EFData.Repositories;

public class FirmRepository : IFirmRepository
{
    private readonly AppDbContext _appDbContext;

    public FirmRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public IQueryable<Firm> Firms => _appDbContext.Firms
        .AsNoTracking()
        .Include(f => f.OwnershipSystem)
        .Include(f => f.TaxationSystem)
        .Include(f => f.Manager);

    public IQueryable<OwnershipSystem> OwnershipSystems => _appDbContext.OwnershipSystems.AsNoTracking();

    public IQueryable<TaxationSystem> TaxationSystems => _appDbContext.TaxationSystems.AsNoTracking();
}
