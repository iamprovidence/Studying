using DataAccessLayer.Entities;
using DataAccessLayer.EntitiesConfiguration;

using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Context
{
    public class BinaryDbContext : DbContext
    {
        // FIELDS
        Interfaces.IDbInitializer dbInitializer;

        // CONSTRUCTORS
        public BinaryDbContext(DbContextOptions<BinaryDbContext> options, Interfaces.IDbInitializer dbInitializer) 
            : base(options)
        {
            this.dbInitializer = dbInitializer;
            Database.Migrate();
        }
        
        // BD SETS
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskState> TaskStates { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<User> Users { get; set; }

        // METHODS
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // configure entities
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectConfiguration());
            modelBuilder.ApplyConfiguration(new TaskConfiguration());
            modelBuilder.ApplyConfiguration(new TaskStateConfiguration());
            modelBuilder.ApplyConfiguration(new TeamConfiguration());

            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(BinaryDbContext).Assembly);
            
            // adds data
            dbInitializer?.Seed(modelBuilder);
        }
    }
}
