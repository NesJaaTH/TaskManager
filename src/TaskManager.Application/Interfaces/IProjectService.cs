using TaskManager.Application.DTOs;

namespace TaskManager.Application.Interfaces
{
    public interface IProjectService
    {
        Task<ProjectResponseDto> CreateProjectAsync(CreateProjectDto dto, Guid UserId);
        Task<ProjectResponseDto> FindProjectByIdAsync(Guid id, Guid UserId);
        Task<ProjectResponseDto> UpdateProjectByIdAsync(UpdateProjectDto dto);
        Task<string> DeletedProjectByIdAsync(Guid id, Guid userId);
    }
}
