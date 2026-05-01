using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Ports
{
    public interface IProjectsRepository
    {
        Task<Project> CreateProjectAsync(Project project);
        Task<Project?> FindProjectByIdAsync(Guid id);
        Task<string?> DeletedProjectsByUserIdAsync(Guid id);
        Task<Project> UpdateProjectsByUserIdAsync(Project project);
    }
}
