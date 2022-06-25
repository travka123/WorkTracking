using Entities;
using UseCases.Repositories;

namespace UseCases.UseCases;

public class UserGetFirmsInteractor : IRequestHandler<int, IQueryable<Firm>>
{
    private readonly IFirmRepository _frimRepository;

    public UserGetFirmsInteractor(IFirmRepository frimRepository)
    {
        _frimRepository = frimRepository;
    }

    public IQueryable<Firm> Handle(int actorId)
    {
        return _frimRepository.Firms.Where(f => f.ManagerId == actorId);
    }
}
