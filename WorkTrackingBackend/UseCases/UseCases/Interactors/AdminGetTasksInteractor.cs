using Entities;
using UseCases.Repositories;

namespace UseCases.UseCases.Interactors;

public class AdminGetTasksInteractor : IRequestHandler<int, IQueryable<AccountableTask>>
{
    private readonly ITaskRepository _taskRepository;

    public AdminGetTasksInteractor(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public IQueryable<AccountableTask> Handle(int actorId)
    {
        return _taskRepository.AccountableTasks.Where(t => t.Firm.Manager.AdministratorId == actorId);
    }
}
