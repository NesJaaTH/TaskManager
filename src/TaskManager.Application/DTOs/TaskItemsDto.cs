using TaskManager.Domain.Entities;

namespace TaskManager.Application.DTOs
{
    public class CreateTaskItemsDto
    {
        public Guid? AssigneeId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskItemsStatus Status { get; set; } = TaskItemsStatus.ToDo;
        public TaskItemsPriority Priority { get; set; } = TaskItemsPriority.Low;
        public DateTime? DueDate { get; set; }
    }

    public class ResponseTaskItemsDto
    {
        public Guid Id { get; set; }
        public Guid? AssigneeId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskItemsStatus Status { get; set; }
        public TaskItemsPriority Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class UpdateTaskItemsDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public TaskItemsStatus? Status { get; set; }
        public TaskItemsPriority? Priority { get; set; }
        public DateTime? DueDate { get; set; }
    }

    public class TaskItemsFilterDto
    {
        public Guid? AssigneeId { get; set; }
        public TaskItemsStatus? Status { get; set; }
        public TaskItemsPriority? Priority { get; set; }
        public DateTime? DueDateFrom { get; set; }
        public DateTime? DueDateTo { get; set; }
    }
}
