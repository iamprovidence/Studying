using DataAccessLayer.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.EntitiesConfiguration
{
    internal class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            // VALIDATION

            // id
            builder.HasKey(p => p.Id);              

            // name
            builder.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(25);
            // description
            builder.Property(p => p.Description)
                    .IsRequired(false)
                    .HasMaxLength(150);

            // FOREIGN KEYS

            // tasks
            builder.HasMany(p => p.Tasks)
                    .WithOne(t => t.Project)
                    .HasForeignKey(p => p.TaskStateId)
                    .OnDelete(DeleteBehavior.Cascade);

            // authors
            builder.HasOne(p => p.Author)
                    .WithMany(a => a.Projects)
                    .HasForeignKey(p => p.AuthorId)
                    .OnDelete(DeleteBehavior.Cascade);

            // teams
            builder.HasOne(p => p.Team)
                    .WithMany(t => t.Projects)
                    .HasForeignKey(p => p.TeamId)
                    .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
