using Entities;
using UseCases.Repositories;

namespace EFData.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<AccountableTask> AccountableTasks => throw new NotImplementedException();

        public void AddTask(Task task)
        {
            throw new NotImplementedException();
        }

        public void UpdateTask(Task task)
        {
            throw new NotImplementedException();
        }
    }
}
