using DataAccessLayer.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.EntitiesConfiguration
{
    internal class TaskStateConfiguration : IEntityTypeConfiguration<TaskState>
    {
        public void Configure(EntityTypeBuilder<TaskState> builder)
        {
            // VALIDATION
            // id
            builder.HasKey(ts => ts.Id);

            // value
            builder.Property(ts => ts.Value)
                    .HasMaxLength(25)
                    .IsRequired();

            // FOREIGN KEY
            // tasks
            builder.HasMany(ts => ts.Tasks)
                    .WithOne(t => t.TaskState)
                    .HasForeignKey(t => t.TaskStateId)
                    .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
