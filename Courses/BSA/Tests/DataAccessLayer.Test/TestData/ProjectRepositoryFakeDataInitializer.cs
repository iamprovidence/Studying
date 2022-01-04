using DataAccessLayer.Entities;

using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Test.TestData
{
    class ProjectRepositoryFakeDataInitializer : Interfaces.IDbInitializer
    {
        public int ProjectsAmount => 5;
        public int CalledProjectAmount => 2;
        public int CalledNameAmount => 3;

        public void Seed(ModelBuilder modelBuilder)
        {
            Project project1 = new Project { Id = 1, Name = "Project 1", CreatedAt = System.DateTime.Now, Deadline = System.DateTime.Now.AddMonths(5), Description = "Description" };
            Project project2 = new Project { Id = 2, Name = "Project 2", CreatedAt = System.DateTime.Now, Deadline = System.DateTime.Now.AddMonths(5), Description = "Description" };
            Project project3 = new Project { Id = 3, Name = "Name 3", CreatedAt = System.DateTime.Now, Deadline = System.DateTime.Now.AddMonths(5), Description = "Description" };
            Project project4 = new Project { Id = 4, Name = "Name 4", CreatedAt = System.DateTime.Now, Deadline = System.DateTime.Now.AddMonths(5), Description = "Description" };
            Project project5 = new Project { Id = 5, Name = "Name 5", CreatedAt = System.DateTime.Now, Deadline = System.DateTime.Now.AddMonths(5), Description = "Description" };

            modelBuilder.Entity<Project>().HasData(project1, project2, project3, project4, project5);
        }
    }
}
