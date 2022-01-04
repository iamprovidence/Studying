using Microsoft.EntityFrameworkCore;

using DataAccessLayer.Entities;

namespace DataAccessLayer.Initializers
{
    internal class DefaultDbInitializer : Interfaces.IDbInitializer
    {
        // FIELDS
        Interfaces.IDataProvider dataProvider;

        // CONSTRUCTORS
        public DefaultDbInitializer(Interfaces.IDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;
        }

        public void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskState>().HasData(dataProvider.TaskStates);
            modelBuilder.Entity<Project>().HasData(dataProvider.Projects);
            modelBuilder.Entity<Task>().HasData(dataProvider.Tasks);
            modelBuilder.Entity<Team>().HasData(dataProvider.Teams);
            modelBuilder.Entity<User>().HasData(dataProvider.Users);
        }
    }
}
