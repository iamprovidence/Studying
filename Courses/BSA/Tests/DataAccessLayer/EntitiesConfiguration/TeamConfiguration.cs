using DataAccessLayer.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.EntitiesConfiguration
{
    internal class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            // VALIDATION

            // id
            builder.HasKey(t => t.Id);

            // name
            builder.Property(t => t.Name)
                    .HasMaxLength(25)
                    .IsRequired();

            // FOREIGN KEYS
            // projects
            builder.HasMany(t => t.Projects)
                    .WithOne(p => p.Team)
                    .HasForeignKey(p => p.TeamId)
                    .OnDelete(DeleteBehavior.SetNull);

            // users
            builder.HasMany(t => t.Users)
                    .WithOne(u => u.Team)
                    .HasForeignKey(u => u.TeamId)
                    .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
