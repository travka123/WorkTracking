using Entities;
using UseCases.Repositories;

namespace UseCases.UseCases;

public class GetUnitsInteractor : IRequestHandler<int, IEnumerable<Unit>>
{
    private readonly ITaskRepository _taskRepository;

    public GetUnitsInteractor(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public IEnumerable<Unit> Handle(int actorId)
    {
        return _taskRepository.Units.ToList();
    }
}
