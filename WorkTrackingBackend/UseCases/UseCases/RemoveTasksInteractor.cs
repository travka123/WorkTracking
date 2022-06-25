using Entities;
using UseCases.Repositories;

namespace UseCases.UseCases;

public class RemoveTasksInteractor : IRequestHandler<RemoveTasksRequest, bool>
{
    private readonly ITaskRepository _taskRepository;

    public RemoveTasksInteractor(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public bool Handle(RemoveTasksRequest request)
    {
        var nAvailable = request.getTasksInteractor.Handle(request.actorId)
            .Where(t => request.tasksIds.Contains(t.Id))
            .Count();

        if (request.tasksIds.Count() != nAvailable)
        {
            return false;
        }

        _taskRepository.RemoveTaskRange(request.tasksIds);
        return true;
    }
}

public record RemoveTasksRequest(IRequestHandler<int, IQueryable<AccountableTask>> getTasksInteractor,
    IEnumerable<int> tasksIds, int actorId);