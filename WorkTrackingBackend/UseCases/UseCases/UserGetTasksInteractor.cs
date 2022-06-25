using Entities;
using UseCases.Repositories;

namespace UseCases.UseCases;

public class UserGetTasksInteractor : IRequestHandler<int, IQueryable<AccountableTask>>
{
    private readonly ITaskRepository _taskRepository;

    public UserGetTasksInteractor(ITaskRepository accountableTaskRepository)
    {
        _taskRepository = accountableTaskRepository;
    }

    public IQueryable<AccountableTask> Handle(int actorId)
    {
        return _taskRepository.AccountableTasks.Where(t => t.Firm.ManagerId == actorId);
    }
}