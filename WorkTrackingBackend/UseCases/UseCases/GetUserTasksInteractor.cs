using Entities;
using UseCases.Repositories;

namespace UseCases.UseCases;

public class GetUserTasksInteractor : IRequestHandler<GetUserTasksRequest, GetUserTasksResponse>
{
    private readonly ITaskRepository _accountableTaskRepository;

    public GetUserTasksInteractor(ITaskRepository accountableTaskRepository)
    {
        _accountableTaskRepository = accountableTaskRepository;
    }

    public GetUserTasksResponse Handle(GetUserTasksRequest request)
    {
        return new GetUserTasksResponse(_accountableTaskRepository.AccountableTasks.ToList());
    }
}

public record GetUserTasksRequest(int userId);
public record GetUserTasksResponse(IEnumerable<AccountableTask> tasks);