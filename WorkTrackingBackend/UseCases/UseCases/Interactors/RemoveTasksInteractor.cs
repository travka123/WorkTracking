using Entities;
using UseCases.Repositories;

namespace UseCases.UseCases.Interactors;

public class RemoveTasksInteractor : IRequestHandler<RemoveTasksRequest, bool>
{
    private readonly ITaskRepository _taskRepository;

    public RemoveTasksInteractor(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public bool Handle(RemoveTasksRequest request)
    {
        var nRequested = request.tasksIds.Count();

        var nAvailable = request.getTasksInteractor.Handle(request.actorId)
            .Where(t => request.tasksIds.Contains(t.Id))
            .Count();

        if (nRequested != nAvailable) throw new UseCaseExeption("invalid tasks ids");

        _taskRepository.RemoveTaskRange(request.tasksIds);

        return true;
    }
}

public record RemoveTasksRequest(IRequestHandler<int, IQueryable<AccountableTask>> getTasksInteractor,
    IEnumerable<int> tasksIds, int actorId);