using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Domain.Entities
{
    [Table("Project")]
    public class Project
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public User? Owner { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ProjectStatus Status { get; set; } = ProjectStatus.Active;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<TaskItems> Tasks { get; set; } = new List<TaskItems>();
        public ICollection<ProjectMembers> Members { get; set; } = new List<ProjectMembers>();
    }

    [Table("ProjectMember")]
    public class ProjectMembers
    {
        public Guid ProjectId { get; set; }
        public Project? Project { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public ProjectMemberRole Role { get; set; } = ProjectMemberRole.Owner;
        public DateTime JoinedAt { get; set; }
    }

    public enum ProjectMemberRole
    {
        Owner,
        Member,
    }

    public enum ProjectStatus
    {
        Active,
        Archived
    }
}