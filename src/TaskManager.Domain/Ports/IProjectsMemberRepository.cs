using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Ports
{
    public interface IProjectsMemberRepository
    {
        Task<ProjectMembers> CreateProjectAsync(ProjectMembers project);
    }
}
