using Entities;
using Microsoft.EntityFrameworkCore;
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

        public IQueryable<AccountableTask> AccountableTasks =>
            _context.Tasks
                .AsNoTracking()
                .Include(t => t.Unit)
                .Include(t => t.Firm);

        public IQueryable<Unit> Units => _context.Units.AsNoTracking();

        public void AddTask(AccountableTask task)
        {
            _context.Tasks.Add(task);
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
        }

        public void RemoveTaskRange(IEnumerable<AccountableTask> tasks)
        {
            _context.Tasks.RemoveRange(tasks);
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
        }

        public void UpdateTaskRange(IEnumerable<AccountableTask> tasks)
        {
            _context.Tasks.UpdateRange(tasks);
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
        }
    }
}
