
using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Ports;

namespace TaskManager.Infrastructure.Persistence
{
    public class ProjectMember : IProjectsMemberRepository
    {
        private readonly AppDbContext _db;

        public ProjectMember(AppDbContext db) => _db = db;

        //public async Task<Project?> FindProjectByIdAsync(Guid id) =>
        //    await _db.Projects.FindAsync(id);

        //public async Task<List<Project>> ListProjectByIdAsync(Guid userId, int page = 1, int pageSize = 10)
        //{
        //    var query = _db.Projects.Where(p => p.OwnerId == userId || p.Members.Any(m => m.UserId == userId));

        //    var totalItems = await query.CountAsync();
        //    var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        //    return await query
        //        .OrderByDescending(p => p.CreatedAt)
        //        .Skip((page - 1) * pageSize)
        //        .Take(pageSize)
        //        .ToListAsync();
        //}

        //public async Task<int> CountProjectsByUserIdAsync(Guid userId) =>
        //    await _db.Projects.CountAsync(p => p.OwnerId == userId || p.Members.Any(m => m.UserId == userId));

        //public async Task<Project> UpdateProjectsByUserIdAsync(Project project)
        //{
        //    _db.Projects.Update(project);
        //    await _db.SaveChangesAsync();
        //    return project;
        //}

        //public async Task<string?> DeletedProjectsByUserIdAsync(Guid projectid)
        //{
        //    await _db.Projects.Where(p => p.Id == projectid)
        //        .ExecuteDeleteAsync();
        //    return $"Deleted Id : {projectid}";
        //}

        public async Task<ProjectMembers> CreateProjectAsync(ProjectMembers project)
        {
            _db.ProjectsMember.Add(project);
            await _db.SaveChangesAsync();
            return project;
        }
    }
}
