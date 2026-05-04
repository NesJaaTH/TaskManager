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
    }
}
