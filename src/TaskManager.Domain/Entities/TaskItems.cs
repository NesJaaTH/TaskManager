using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Domain.Entities
{
    [Table(name: "TaskItem")]
    public class TaskItems
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Project? Project { get; set; }
        public Guid? AssigneeId { get; set; }
        public User? Assignee { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskItemsStatus Status { get; set; } = TaskItemsStatus.ToDo;
        public TaskItemsPriority Priority { get; set; } = TaskItemsPriority.Low;
        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }

    public enum TaskItemsStatus
    {
        ToDo,
        InProgress,
        Done,
        Canceled
    }

    public enum TaskItemsPriority
    {
        Low,
        Medium,
        High
    }
}