using Entities;
using UseCases.Repositories;

namespace UseCases.UseCases;

public class UpdateTasksInteractor : IRequestHandler<UpdateTasksRequest, bool>
{
    private readonly ITaskRepository _taskRepository;

    public UpdateTasksInteractor(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public bool Handle(UpdateTasksRequest request)
    {
        var tasksIds = request.updates.Select(t => t.id).ToArray();

        var oldTasks = request.getTasksInteractor.Handle(request.actorId)
            .Where(t => tasksIds.Contains(t.Id))
            .ToDictionary(t => t.Id, t => t);

        if (tasksIds.Count() != oldTasks.Count())
        {
            return false;
        }

        var firmsIds = request.updates.Where(u => u.firmId.HasValue).Select(u => u.firmId!.Value).ToArray();

        var nFrimsAvalible = request.getFirmsInteractor.Handle(request.actorId)
            .Where(t => firmsIds.Contains(t.Id))
            .Count();

        if (nFrimsAvalible != firmsIds.Count())
        {
            return false;
        }

        try
        {
            _taskRepository.UpdateTaskRange(request.updates.Select(u =>
            {
                var oldTask = oldTasks[u.id];
                return new AccountableTask()
                {
                    Id = u.id,
                    Name = u.name ?? oldTask.Name,
                    UnitId = u.unitId ?? oldTask.UnitId,
                    Quantity = u.quantity ?? oldTask.Quantity,
                    Description = u.description ?? oldTask.Description,
                    ReportingDate = u.reportingDate ?? oldTask.ReportingDate,
                    FirmId = u.firmId ?? oldTask.FirmId,
                    CreationDate = oldTask.CreationDate
                };
            }));
        }
        catch
        {
            return false;
        }

        return true;
    }
}


public record UpdateTasksRequest(IRequestHandler<int, IQueryable<AccountableTask>> getTasksInteractor,
    IRequestHandler<int, IQueryable<Firm>> getFirmsInteractor, IEnumerable<TaskUpdate> updates,
    int actorId);

public record TaskUpdate(int id, string? name, int? unitId, int? quantity, string? description, DateTime? reportingDate, int? firmId);