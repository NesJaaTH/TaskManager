using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Ports
{
    public interface ITaskItemsRepository
    {
        Task<TaskItems> CreateTaskAsync(TaskItems taskItem);
        //Task<TaskItems?> FindTaskByProjectIdAsync(Guid id);
        //Task<string?> DeletedTaskByIdAsync(Guid id);
        //Task<int> CountProjectsByUserIdAsync(Guid userid);
        //Task<List<TaskItems>> ListTaskByProjectIdAsync(Guid userid,int page, int pageSize);
        //Task<TaskItems> UpdateTaskByIdAsync(TaskItems taskItem);
    }
}
