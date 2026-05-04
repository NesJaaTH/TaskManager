namespace TaskManager.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public ICollection<Project> OwnedProjects { get; set; } = new List<Project>();
        public ICollection<ProjectMembers> ProjectsMember { get; set; } = new List<ProjectMembers>();
        public ICollection<TaskItems> AssignedTasks { get; set; } = new List<TaskItems>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
