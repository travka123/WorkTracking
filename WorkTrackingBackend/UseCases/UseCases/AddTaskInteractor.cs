using Entities;
using UseCases.Repositories;

namespace UseCases.UseCases;

public class AddTaskInteractor : IRequestHandler<AddTaskRequest, AddTaskResponse>
{ 
    private readonly ITaskRepository _taskRepository;

    public AddTaskInteractor(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public AddTaskResponse Handle(AddTaskRequest request)
    {
        var firm = request.getFirmsInteractor.Handle(request.actorId)
            .SingleOrDefault(f => f.Id == request.firmId);

        if (firm is null)
        {
            return new AddTaskResponse(null);
        }

        var task = new AccountableTask
        {
            Name = request.name,
            UnitId = request.unitId,
            Quantity = request.quantity,
            Description = request.description,
            ReportingDate = request.reportingDate,
            FirmId = request.firmId,
            CreationDate = DateTime.Now
        };

        try
        {
            _taskRepository.AddTask(task);
        }
        catch
        {
            return new AddTaskResponse(null);
        }

        return new AddTaskResponse(_taskRepository.AccountableTasks.SingleOrDefault(t => t.Id == task.Id));
    }
}

public record AddTaskRequest(IRequestHandler<int, IQueryable<Firm>> getFirmsInteractor,
    string name, int unitId, int quantity, string description, DateTime reportingDate, int firmId, int actorId);
public record AddTaskResponse(AccountableTask? task);
