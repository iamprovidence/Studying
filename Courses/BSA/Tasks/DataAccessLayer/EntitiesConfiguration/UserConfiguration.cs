using DataAccessLayer.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.EntitiesConfiguration
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // VALIDATION
            // id
            builder.HasKey(u => u.Id);

            // first name
            builder.Property(u => u.FirstName)
                    .HasMaxLength(25)
                    .IsRequired();

            // last name
            builder.Property(u => u.LastName)
                    .HasMaxLength(25)
                    .IsRequired();

            // email
            builder.Property(u => u.Email)
                    .HasMaxLength(80)
                    .IsRequired();

            // FOREIGN KEYS
            // team
            builder.HasOne(u => u.Team)
                    .WithMany(t => t.Users)
                    .HasForeignKey(u => u.TeamId)
                    .OnDelete(DeleteBehavior.SetNull);

            // projects
            builder.HasMany(u => u.Projects)
                    .WithOne(p => p.Author)
                    .HasForeignKey(p => p.AuthorId)
                    .OnDelete(DeleteBehavior.Cascade);

            // tasks
            builder.HasMany(u => u.Tasks)
                    .WithOne(t => t.Performer)
                    .HasForeignKey(t => t.PerformerId)
                    .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
