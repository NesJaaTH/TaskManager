using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Ports;

namespace TaskManager.Application.Services
{
    public class TaskItemsService : ITaskItemsService
    {
        private readonly ITaskItemsRepository _taskItemsRepo;

        public TaskItemsService(ITaskItemsRepository taskItemsRepo)
        {
            _taskItemsRepo = taskItemsRepo;
        }

        public async Task<ResponseTaskItemsDto> CreateTaskAsync(CreateTaskItemsDto dto, Guid projectId)
        {
            var TaskItems = new TaskItems
            {
                ProjectId = projectId,
                AssigneeId = dto.AssigneeId ?? null,
                Title = dto.Title,
                Description = dto.Description,
                Status = dto.Status,
                Priority = dto.Priority,
                DueDate = dto.DueDate ?? null,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdTaskItems = await _taskItemsRepo.CreateTaskAsync(TaskItems);

            return new ResponseTaskItemsDto
            {
                Id = createdTaskItems.Id,
                AssigneeId = createdTaskItems.AssigneeId,
                Title = createdTaskItems.Title,
                Description = createdTaskItems.Description,
                Status = createdTaskItems.Status,
                Priority = createdTaskItems.Priority,
                DueDate = createdTaskItems.DueDate,
                CreatedAt = createdTaskItems.CreatedAt,
                UpdatedAt = createdTaskItems.UpdatedAt
            };
        }

        public async Task<List<ResponseTaskItemsDto>> ListTaskByProjectIdAsync(Guid projectId)
        {
            var taskItemsList = await _taskItemsRepo.ListTaskByProjectIdAsync(projectId);
            return taskItemsList.Select(task => new ResponseTaskItemsDto
            {
                Id = task.Id,
                AssigneeId = task.AssigneeId,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                Priority = task.Priority,
                DueDate = task.DueDate,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt
            }).ToList();
        }

        public async Task<ResponseTaskItemsDto> UpdateTaskItemsByIdAsync(UpdateTaskItemsDto dto, Guid taskItemId)
        {
            var existingTaskItem = await _taskItemsRepo.FindTaskByIdAsync(taskItemId);
            if (existingTaskItem == null)
            {
                throw new ArgumentException("Task item not found.");
            }
            existingTaskItem.Title = dto.Title ?? existingTaskItem.Title;
            existingTaskItem.Description = dto.Description ?? existingTaskItem.Description;
            existingTaskItem.Status = dto.Status ?? existingTaskItem.Status;
            existingTaskItem.Priority = dto.Priority ?? existingTaskItem.Priority;
            existingTaskItem.DueDate = dto.DueDate ?? existingTaskItem.DueDate;
            existingTaskItem.UpdatedAt = DateTime.UtcNow;
            var updatedTaskItem = await _taskItemsRepo.UpdateTaskByIdAsync(existingTaskItem);
            return new ResponseTaskItemsDto
            {
                Id = updatedTaskItem.Id
            };
        }

        public async Task<string?> DeletedTaskByIdAsync(Guid taskItemId)
        {
            var existingTaskItem = await _taskItemsRepo.FindTaskByIdAsync(taskItemId);
            if (existingTaskItem == null)
            {
                throw new ArgumentException("Task item not found.");
            }
            await _taskItemsRepo.DeletedTaskByIdAsync(taskItemId);
            return $"Task item with ID {taskItemId} has been deleted.";
        }
    }
}
