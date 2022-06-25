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

        public void RemoveTaskRange(IEnumerable<int> tasksIds)
        {
            var target = tasksIds.Select(id => new AccountableTask { Id = id }).ToList();

            _context.Tasks.RemoveRange(target);
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
        }

        public void UpdateTaskRange(IEnumerable<AccountableTask> tasks)
        {
            var target = tasks.Select(t => new AccountableTask
            {
                Id = t.Id,
                Name = t.Name,
                CreationDate = t.CreationDate,
                Description = t.Description,
                Quantity = t.Quantity,
                ReportingDate = t.ReportingDate,
                FirmId = t.FirmId,
                UnitId = t.UnitId,
            });

            _context.Tasks.UpdateRange(target);
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
        }
    }
}
