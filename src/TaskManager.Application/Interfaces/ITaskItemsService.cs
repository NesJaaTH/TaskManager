using TaskManager.Application.DTOs;

namespace TaskManager.Application.Interfaces
{
    public interface ITaskItemsService
    {
        Task<ResponseTaskItemsDto> CreateTaskAsync(CreateTaskItemsDto taskItem, Guid projectId);
        //Task<TaskItem?> FindTaskByProjectIdAsync(Guid id);
        Task<string?> DeletedTaskByIdAsync(Guid taskItemId);
        ////Task<int> CountProjectsByUserIdAsync(Guid userid);
        Task<List<ResponseTaskItemsDto>> ListTaskByProjectIdAsync(Guid projectId);
        Task<ResponseTaskItemsDto> UpdateTaskItemsByIdAsync(UpdateTaskItemsDto taskItems, Guid taskItemId);
    }
}
