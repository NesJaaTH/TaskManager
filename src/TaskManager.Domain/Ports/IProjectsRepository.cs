using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Ports
{
    public interface IProjectsRepository
    {
        Task<Project> CreateProjectAsync(Project project);
        Task<Project?> FindProjectByIdAsync(Guid id);
        Task<string?> DeletedProjectsByUserIdAsync(Guid id);
        Task<int> CountProjectsByUserIdAsync(Guid userid);
        Task<List<Project>> ListProjectByIdAsync(Guid userid,int page, int pageSize);
        Task<Project> UpdateProjectsByUserIdAsync(Project project);
    }
}
