namespace TaskManager.Domain.Entities
{
    public class Comment
    {
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public TaskItems? Task { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}