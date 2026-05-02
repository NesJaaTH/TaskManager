using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Ports;

namespace TaskManager.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectsRepository _projectsRepo;
        private readonly IProjectsMemberRepository _projectMemberRepo;

        public ProjectService(IProjectsRepository projectsRepo, IProjectsMemberRepository projectMemberRepo)
        {
            _projectsRepo = projectsRepo;
            _projectMemberRepo = projectMemberRepo;
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

            var createdMember = await _projectMemberRepo.CreateProjectAsync(new ProjectMembers
            {
                ProjectId = createdProject.Id,
                UserId = UserId,
                Role = ProjectMemberRole.Owner,
                JoinedAt = DateTime.UtcNow
            });

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

        public async Task<Pagination<ProjectResponseDto>> ListProjectByIdAsync(Guid userId, int page = 1, int pageSize = 10)
        {
            var projects = await _projectsRepo.ListProjectByIdAsync(userId, page, pageSize);
            var totalItems = await _projectsRepo.CountProjectsByUserIdAsync(userId);
            if (!projects.Any())
                return new Pagination<ProjectResponseDto>(new List<ProjectResponseDto>(), 0, 0, page, pageSize);

            var projectDtos = projects.Select(p => new ProjectResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            }).ToList();

            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            return new Pagination<ProjectResponseDto>(projectDtos, totalItems, totalPages, page, pageSize);
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
