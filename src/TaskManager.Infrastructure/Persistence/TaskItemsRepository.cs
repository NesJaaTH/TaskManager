using TaskManager.Domain.Entities;
using TaskManager.Domain.Ports;

namespace TaskManager.Infrastructure.Persistence
{
    public class TaskItemsRepository : ITaskItemsRepository
    {
        private readonly AppDbContext _db;

        public TaskItemsRepository(AppDbContext db) => _db = db;

        public async Task<TaskItems> CreateTaskAsync(TaskItems item) 
        {
            _db.TasksItems.Add(item);
            await _db.SaveChangesAsync();
            return item;
        }
    }
}
