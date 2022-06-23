using Entities;

namespace UseCases.Repositories;

public interface IFirmRepository
{
    public IQueryable<Firm> Firms { get; }
    public IQueryable<OwnershipSystem> OwnershipSystems { get; }
    public IQueryable<TaxationSystem> TaxationSystems { get; }
}
