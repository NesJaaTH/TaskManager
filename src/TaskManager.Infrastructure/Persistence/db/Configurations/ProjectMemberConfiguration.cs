using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Persistence.db.Configurations
{
    public class ProjectMemberConfiguration : IEntityTypeConfiguration<ProjectMembers>
    {
        public void Configure(EntityTypeBuilder<ProjectMembers> e) 
        {
            e.HasKey(pm => new { pm.ProjectId, pm.UserId });
            e.HasOne(pm => pm.Project)
                .WithMany(p => p.Members)
                .HasForeignKey(pm => pm.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(pm => pm.User)
                .WithMany(u => u.ProjectsMember)
                .HasForeignKey(pm => pm.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            e.Property(pm => pm.Role)
                .HasConversion<string>();
        }
    }
}
