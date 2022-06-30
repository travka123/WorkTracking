using Entities;
using UseCases.Repositories;

namespace UseCases.UseCases.Interactors
{
    public class AdminGetFirmsInteractor : IRequestHandler<int, IQueryable<Firm>>
    {
        private readonly IFirmRepository _firmRepository;

        public AdminGetFirmsInteractor(IFirmRepository firmRepository)
        {
            _firmRepository = firmRepository;
        }

        public IQueryable<Firm> Handle(int actorId)
        {
            return _firmRepository.Firms.Where(f => f.Manager.AdministratorId == actorId);
        }
    }
}
