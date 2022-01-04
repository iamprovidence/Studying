using DataAccessLayer.Entities;

using Microsoft.EntityFrameworkCore;

using Date = System.DateTime;


namespace DataAccessLayer.Test.TestData
{
    class TaskRepositoryFakeDataInitializer : Interfaces.IDbInitializer
    {
        public int TotalEntitiesCount => 7;
        public int TotalUserProjectAmount => 2;
        public int TotalUserProjectStatusAmount => 1;
        public Task LongestTask { get; private set; }
        public Task WithLongestDescriptionForSecondProject { get; private set; }
        public Task WithShortestNameForSecondUser { get; private set; }

        public void Seed(ModelBuilder modelBuilder)
        {
            Task task1 = new Task { Id = 1, Name = "Task 1", Description = "Descr", CreatedAt = Date.Now, FinishedAt = Date.Now.AddMonths(5) };
            Task task2 = new Task { Id = 2, Name = "Task 2", Description = "Descri", CreatedAt = Date.Now, FinishedAt = Date.Now.AddMonths(5), PerformerId = 2 };
            Task task3 = new Task { Id = 3, Name = "Task 3", Description = "Descrip", CreatedAt = Date.Now, FinishedAt = Date.Now.AddMonths(5), PerformerId = 2 };
            Task task4 = new Task { Id = 4, Name = "Task4", Description = "Descript", CreatedAt = Date.Now, FinishedAt = Date.Now.AddMonths(5), ProjectId = 2 };
            Task task5 = new Task { Id = 5, Name = "Task 5", Description = "Descripti", CreatedAt = Date.Now, FinishedAt = Date.Now.AddMonths(5), ProjectId = 2 };
            Task task6 = new Task { Id = 6, Name = "Task 6", Description = "Descriptio", CreatedAt = Date.Now, FinishedAt = Date.Now.AddMonths(5), PerformerId = 1, ProjectId = 1, TaskStateId = 1 };
            Task task7 = new Task { Id = 7, Name = "Task 7", Description = "Description", CreatedAt = Date.Now, FinishedAt = Date.Now.AddMonths(15), PerformerId = 1, ProjectId = 1, TaskStateId = 2 };

            LongestTask = task7;
            WithLongestDescriptionForSecondProject = task5;
            WithShortestNameForSecondUser = task4;

            modelBuilder.Entity<Task>().HasData(task1, task2, task3, task4, task5, task6, task7);
        }
    }
}
