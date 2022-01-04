using System.Collections.Generic;

using System.Linq;

using DataAccessLayer.Entities;

using Microsoft.EntityFrameworkCore;

namespace Server.IntegratedTests.TestData
{
    public class FakeDataInitializer : DataAccessLayer.Interfaces.IDbInitializer
    {
        // FIELDS
        User[] users;
        Task[] tasks;
        Project[] projects;
        Team[] teams;
        TaskState[] taskStates;

        // CONSTRUCTORS
        public FakeDataInitializer()
        {
            users = BuildUsers();
            tasks = BuildTasks();
            teams = BuildTeams();
            projects = BuildProjects();
            taskStates = BuildTaskStates();
        }

        // PROPERTIES
        public IEnumerable<User> Users => users;
        public IEnumerable<Task> Tasks => tasks;
        public IEnumerable<Project> Projects => projects;
        public IEnumerable<Team> Teams => teams;
        public IEnumerable<TaskState> TaskStates => taskStates;

        // METHODS
        public void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskState>().HasData(TaskStates);
            modelBuilder.Entity<Project>().HasData(Projects);
            modelBuilder.Entity<Task>().HasData(Tasks);
            modelBuilder.Entity<Team>().HasData(Teams);
            modelBuilder.Entity<User>().HasData(Users);
        }
        // this method is required because InMemoryDb 
        // does not know how to increase ID
        //
        // so we do not seed data, but add it regularly
        // I delete ids for our custom generator to work
        // also I left regular implementation with Seed, just in case I needed it
        public void Seed(DataAccessLayer.Context.BinaryDbContext dbContext)
        {
            dbContext.TaskStates.AddRange(TaskStates.Select(ResetId));
            dbContext.Projects.AddRange(Projects.Select(ResetId));
            dbContext.Tasks.AddRange(Tasks.Select(ResetId));
            dbContext.Teams.AddRange(Teams.Select(ResetId));
            dbContext.Users.AddRange(Users.Select(ResetId));

            dbContext.SaveChanges();
        }
        public TEntity ResetId<TEntity>(TEntity entity)
            where TEntity : DataAccessLayer.Interfaces.IEntity
        {
            entity.Id = 0;
            return entity;
        }
        // BUILDER
        public User[] BuildUsers()
        {
            return new User[]
            {
                new User
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@mail.com",
                    Birthday = System.DateTime.Now.AddMonths(-5),
                    RegisteredAt = System.DateTime.Now.AddDays(10),

                    TeamId = 1
                },
                new User
                {
                    Id = 2,
                    FirstName = "Jane",
                    LastName = "Doe",
                    Email = "jane.doe@mail.com",
                    Birthday = System.DateTime.Now.AddMonths(-6),
                    RegisteredAt = System.DateTime.Now.AddDays(15),

                    TeamId = 2
                }
            };
        }
        public Task[] BuildTasks()
        {
            return new Task[]
            {
                new Task
                {
                    Id = 1,
                    Name = "Task 1",
                    Description = "Task 1",
                    CreatedAt = System.DateTime.Now.AddMonths(-5),
                    FinishedAt = System.DateTime.Now.AddMonths(5),

                    PerformerId = 1,
                    ProjectId = 1,
                    TaskStateId = 1,
                },
                new Task
                {
                    Id = 2,
                    Name = "Task 2",
                    Description = "Task 2",
                    CreatedAt = System.DateTime.Now.AddMonths(-5),
                    FinishedAt = System.DateTime.Now.AddMonths(5),

                    PerformerId = 2,
                    ProjectId = 1,
                    TaskStateId = 2,
                },
                new Task
                {
                    Id = 3,
                    Name = "Task 3",
                    Description = "Task 3",
                    CreatedAt = System.DateTime.Now.AddMonths(-5),
                    FinishedAt = System.DateTime.Now.AddMonths(5),

                    PerformerId = 1,
                    ProjectId = 1,
                    TaskStateId = 3,
                },
                new Task
                {
                    Id = 4,
                    Name = "Task 4",
                    Description = "Task 4",
                    CreatedAt = System.DateTime.Now.AddMonths(-5),
                    FinishedAt = System.DateTime.Now.AddMonths(5),

                    PerformerId = 2,
                    ProjectId = 2,
                    TaskStateId = 3,
                }
            };
        }
        public Project[] BuildProjects()
        {
            return new Project[]
            {
                new Project
                {
                    Id = 1,
                    Name = "Project 1",
                    Description = "Project 1",
                    CreatedAt = System.DateTime.Now.AddMonths(-5),
                    Deadline = System.DateTime.Now.AddMonths(5),

                    AuthorId = 1,
                    TeamId = 1,
                },
                new Project
                {
                    Id = 2,
                    Name = "Project 2",
                    Description = "Project 2",
                    CreatedAt = System.DateTime.Now.AddMonths(-5),
                    Deadline = System.DateTime.Now.AddMonths(5),

                    AuthorId = 2,
                    TeamId = 1,
                }
            };
        }
        public Team[] BuildTeams()
        {
            return new Team[]
            {
                new Team
                {
                    Id = 1,
                    CreatedAt = System.DateTime.Now.AddMonths(-5),
                    Name = "Team 1",
                },
                new Team
                {
                    Id = 2,
                    CreatedAt = System.DateTime.Now.AddMonths(-5),
                    Name = "Team 2",
                }
            };
        }
        public TaskState[] BuildTaskStates()
        {
            return new TaskState[]
            {
                new TaskState { Id = 1, Value = "Created" },
                new TaskState { Id = 2, Value = "Started" },
                new TaskState { Id = 3, Value = "Finished" },
                new TaskState { Id = 4, Value = "Canceled" },
            };
        }
    }
}
