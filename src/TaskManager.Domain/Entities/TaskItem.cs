namespace TaskManager.Domain.Entities
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Project? Project { get; set; }
        public Guid? AssigneeId { get; set; }
        public User? Assignee { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskItemStatus Status { get; set; } = TaskItemStatus.ToDo;
        public TaskItemPriority Priority { get; set; } = TaskItemPriority.Low;
        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }

    public enum TaskItemStatus
    {
        ToDo,
        InProgress,
        Done,
        Canceled
    }

    public enum TaskItemPriority
    {
        Low,
        Medium,
        High
    }
}