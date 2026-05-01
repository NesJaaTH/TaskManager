using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Ports;

namespace TaskManager.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectsRepository _projectsRepo;
        private readonly IUserRepository _userRepo;

        public ProjectService(IProjectsRepository projectsRepo, IUserRepository userRepo)
        {
            _projectsRepo = projectsRepo;
            _userRepo = userRepo;
        }

        public async Task<ProjectResponseDto> CreateProjectAsync(CreateProjectDto dto, Guid UserId)
        {
            var project = new Project
            {
                Name = dto.Name,
                OwnerId = UserId,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            var createdProject = await _projectsRepo.CreateProjectAsync(project);
            return new ProjectResponseDto
            {
                Id = createdProject.Id,
                Name = createdProject.Name,
                Description = createdProject.Description,
                CreatedAt = createdProject.CreatedAt,
                UpdatedAt = createdProject.UpdatedAt
            };
        }

        public async Task<ProjectResponseDto> FindProjectByIdAsync(Guid projectId, Guid userId)
        {
            var project = await _projectsRepo.FindProjectByIdAsync(projectId);
            if (project == null)
                throw new InvalidOperationException("Project not found.");
            // Check if user is owner or member
            if (project.OwnerId != userId && !project.Members.Any(m => m.UserId == userId))
                throw new UnauthorizedAccessException("You do not have access to this project.");
            return new ProjectResponseDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                CreatedAt = project.CreatedAt,
                UpdatedAt = project.UpdatedAt
            };
        }

        public async Task<ProjectResponseDto> UpdateProjectByIdAsync(UpdateProjectDto dto)
        {
            var project = await _projectsRepo.FindProjectByIdAsync(dto.Id);
            if (project == null)
                throw new InvalidOperationException("Project not found.");
            project.Name = dto.Name;
            project.Description = dto.Description;
            project.UpdatedAt = DateTime.UtcNow;
            var updatedProject = await _projectsRepo.UpdateProjectsByUserIdAsync(project);
            return new ProjectResponseDto
            {
                Name = updatedProject!.Name,
                Description = updatedProject.Description,
                CreatedAt = updatedProject.CreatedAt,
                UpdatedAt = updatedProject.UpdatedAt
            };
        }

        public async Task<string> DeletedProjectByIdAsync(Guid id, Guid userId)
        {
            var project = await _projectsRepo.FindProjectByIdAsync(id);
            if (project == null)
                throw new InvalidOperationException("Project not found.");
            // Check if user is owner or member
            if (project.OwnerId != userId && !project.Members.Any(m => m.UserId == userId))
                throw new UnauthorizedAccessException("You do not have access to this project.");
            var deletedProject = await _projectsRepo.DeletedProjectsByUserIdAsync(id);
            return "Project deleted successfully.";
        }
    }
}
