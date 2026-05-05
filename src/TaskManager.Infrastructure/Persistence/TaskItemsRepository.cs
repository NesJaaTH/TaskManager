using Microsoft.EntityFrameworkCore;
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
        public async Task<List<TaskItems>> ListTaskByProjectIdAsync(Guid projectId)
        {
            return await _db.TasksItems.Where(t => t.ProjectId == projectId).ToListAsync();
        }

        public async Task<TaskItems?> FindTaskByIdAsync(Guid TaskItemId)
        {
            var taskItem = await _db.TasksItems.FindAsync(TaskItemId);
            if (taskItem == null)
            {
                throw new KeyNotFoundException($"Task with ID {TaskItemId} not found.");
            }
            return taskItem;
        }

        public async Task<TaskItems> UpdateTaskByIdAsync(TaskItems item)
        {
            var existingTaskItem = await _db.TasksItems.FindAsync(item.Id);
            if (existingTaskItem == null)
            {
                throw new KeyNotFoundException($"Task with ID {item.Id} not found.");
            }
            existingTaskItem.AssigneeId = item.AssigneeId;
            existingTaskItem.Title = item.Title;
            existingTaskItem.Description = item.Description;
            existingTaskItem.Status = item.Status;
            existingTaskItem.Priority = item.Priority;
            existingTaskItem.DueDate = item.DueDate;
            existingTaskItem.UpdatedAt = DateTime.UtcNow;
            _db.TasksItems.Update(existingTaskItem);
            await _db.SaveChangesAsync();
            return existingTaskItem;

        }

        public async Task<string?> DeletedTaskByIdAsync(Guid taskItemId)
        {
            var taskItem = await _db.TasksItems.FindAsync(taskItemId);
            if (taskItem == null)
            {
                throw new KeyNotFoundException($"Task with ID {taskItemId} not found.");
            }
            _db.TasksItems.Remove(taskItem);
            await _db.SaveChangesAsync();

            return $"Task item with ID {taskItemId} has been deleted.";
        }
    }
}
