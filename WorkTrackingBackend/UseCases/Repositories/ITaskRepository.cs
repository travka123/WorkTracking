using Entities;

namespace UseCases.Repositories;

public interface ITaskRepository
{
    public IQueryable<AccountableTask> AccountableTasks { get; }
    public void AddTask(Task task);
    public void UpdateTask(Task task);
}
