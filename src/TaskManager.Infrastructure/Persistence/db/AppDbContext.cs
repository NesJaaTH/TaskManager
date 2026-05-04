using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<ProjectMembers> ProjectsMember => Set<ProjectMembers>();
        public DbSet<TaskItems> TasksItems => Set<TaskItems>();
        public DbSet<Comment> Comments => Set<Comment>();

        protected override void OnModelCreating(ModelBuilder model)
        {
            // โหลด configuration ทุกไฟล์ใน assembly นี้อัตโนมัติ
            model.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}