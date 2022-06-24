using Entities;

namespace UseCases.Repositories;

public interface ITaskRepository
{
    public IQueryable<AccountableTask> AccountableTasks { get; }
    public IQueryable<Unit> Units { get; }
    public void AddTask(AccountableTask task);
    public void UpdateTaskRange(IEnumerable<AccountableTask> tasks);
    public void RemoveTaskRange(IEnumerable<AccountableTask> tasks);
}
