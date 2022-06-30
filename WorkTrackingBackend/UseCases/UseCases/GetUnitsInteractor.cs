using Entities;
using UseCases.Repositories;

namespace UseCases.UseCases;

public class GetUnitsInteractor : IRequestHandler<int, IQueryable<Unit>>
{
    private readonly ITaskRepository _taskRepository;

    public GetUnitsInteractor(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public IQueryable<Unit> Handle(int actorId)
    {
        return _taskRepository.Units;
    }
}
