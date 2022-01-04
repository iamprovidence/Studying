using DataAccessLayer.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.EntitiesConfiguration
{
    internal class TaskConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            // VALIDATION
            // id
            builder.HasKey(t => t.Id);

            // name
            builder.Property(t => t.Name)
                    .HasMaxLength(25)
                    .IsRequired();

            // description
            builder.Property(t => t.Description)
                    .HasMaxLength(150)
                    .IsRequired(false);


            // FOREIGN KEYS
            // project
            builder.HasOne(t => t.Project)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(t => t.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade);

            // performer
            builder.HasOne(t => t.Performer)
                    .WithMany(u => u.Tasks)
                    .HasForeignKey(t => t.PerformerId)
                    .OnDelete(DeleteBehavior.SetNull);

            // state
            builder.HasOne(t => t.TaskState)
                    .WithMany(ts => ts.Tasks)
                    .HasForeignKey(t => t.TaskStateId)
                    .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
