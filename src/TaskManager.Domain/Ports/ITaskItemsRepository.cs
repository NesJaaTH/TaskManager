using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Ports
{
    public interface ITaskItemsRepository
    {
        Task<TaskItems> CreateTaskAsync(TaskItems taskItem);
        Task<TaskItems?> FindTaskByIdAsync(Guid taskItemId);
        Task<string?> DeletedTaskByIdAsync(Guid taskItemId);
        //Task<int> CountProjectsByUserIdAsync(Guid userid);
        Task<List<TaskItems>> ListTaskByProjectIdAsync(Guid projectId);
        Task<TaskItems> UpdateTaskByIdAsync(TaskItems taskItem);
    }
}
