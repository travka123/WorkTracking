using Entities;
using UseCases.Repositories;

namespace UseCases.UseCases;

public class AddTaskInteractor : IRequestHandler<AddTaskRequest, AccountableTask>
{ 
    private readonly ITaskRepository _taskRepository;

    public AddTaskInteractor(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public AccountableTask Handle(AddTaskRequest request)
    {
        var firm = request.getFirmsInteractor.Handle(request.actorId)
            .SingleOrDefault(f => f.Id == request.firmId);

        if (firm is null) throw new UseCaseExeption("invalid task data");
     
        var taskData = new AccountableTask
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
            _taskRepository.AddTask(taskData);
        }
        catch
        {
            throw new UseCaseExeption("invalid task data");
        }

        var task = _taskRepository.AccountableTasks.SingleOrDefault(t => t.Id == taskData.Id);

        if (task is null) throw new UseCaseExeption("task has been deleted");

        return task;
    }
}

public record AddTaskRequest(IRequestHandler<int, IQueryable<Firm>> getFirmsInteractor,
    string name, int unitId, int quantity, string description, DateTime reportingDate, int firmId, int actorId);
