
using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Ports;

namespace TaskManager.Infrastructure.Persistence
{
    public class ProjectRepository : IProjectsRepository
    {
        private readonly AppDbContext _db;

        public ProjectRepository(AppDbContext db) => _db = db;

        public async Task<Project?> FindProjectByIdAsync(Guid id) =>
            await _db.Projects.FindAsync(id);

        //public async Task<User?> FindByEmailAsync(string email) =>
        //    await _db.Users.FirstOrDefaultAsync(u => u.Email == email);

        public async Task<Project> UpdateProjectsByUserIdAsync(Project project)
        {
            _db.Projects.Update(project);
            await _db.SaveChangesAsync();
            return project;
        }

        public async Task<string?> DeletedProjectsByUserIdAsync(Guid projectid)
        {
            await _db.Projects.Where(p => p.Id == projectid)
                .ExecuteDeleteAsync();
            return $"Deleted Id : {projectid}";
        }

        public async Task<Project> CreateProjectAsync(Project project)
        {
            _db.Projects.Add(project);
            await _db.SaveChangesAsync();
            return project;
        }
    }
}
