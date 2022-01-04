using BusinessLayer.DataTransferObjects;

using DataAccessLayer.Entities;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;

using Date = System.DateTime;

namespace BusinessLayer.Test.Helpers
{
    class DefaultFakeDataInitializer : DataAccessLayer.Interfaces.IDbInitializer
    {
        // CONST
        public static readonly int USERS_AMOUNT = 20;
        public static readonly int PROJECTS_AMOUNT = 5;
        public static readonly int TASKS_AMOUNT = 50;
        public static readonly int TEAMS_AMOUNT = 3;

        public static readonly string PROJECT_NAME_FORMAT = "Project {0}";

        // FIELDS
        User[] users;
        Task[] tasks;
        Project[] projects;
        Team[] teams;

        // CONSTRUCTORS
        public DefaultFakeDataInitializer()
        {
            // fields
            users = null;
            tasks = null;
            projects = null;
            teams = null;

            // properties
            Random = new Random();

            ProjectAuthor = new Dictionary<int, int>();

            ProjectTaskAmountPerUsers = new IDictionary<string, int>[USERS_AMOUNT];
            for (int i = 0; i < USERS_AMOUNT; ++i)
            {
                ProjectTaskAmountPerUsers[i] = new Dictionary<string, int>();
            }

            UsersTask = new List<Task>[USERS_AMOUNT];
            for (int i = 0; i < USERS_AMOUNT; ++i)
            {
                UsersTask[i] = new List<Task>();
            }
        }

        // PROPERTIES
        Random Random { get; }

        // key = projectId, value = userId
        public IDictionary<int, int> ProjectAuthor { get; }
        // index = userId
        //      key = projectName, value = taskAmount
        public IDictionary<string, int>[] ProjectTaskAmountPerUsers { get; }
        // index = userId
        //      List =contains of= usersTask
        public List<Task>[] UsersTask { get; }

        public IEnumerable<UserTasksDTO> UsersAndTasks
        {
            get
            {
                return Users
                    .OrderBy(u => u.FirstName)
                    .Select((user, index) =>
                    {
                        return new UserTasksDTO
                        {
                            UserName = $"{user.FirstName} {user.LastName}",
                            TaskNames = UsersTask[index].Select(t => t.Name)
                        };
                    });                
            }
        }
        public IEnumerable<TeamUsersDTO> TeamUsers
        {
            get
            {
                return new TeamUsersDTO[]
                {
                    new TeamUsersDTO
                    {
                        TeamId = 3,
                        TeamName = Teams[TEAMS_AMOUNT - 1].Name,
                        Participants = new User[]
                        {
                            Users[USERS_AMOUNT - 1]
                        }
                    }
                };
            }
        }
        

        // DATA
        public User[] Users
        {
            get
            {
                if (users == null) users = GenerateUsers();
                return users;
            }
        }

        public Task[] Tasks
        {
            get
            {
                if (tasks == null) tasks = GenerateTasks();
                return tasks;
            }
        }

        public Project[] Projects
        {
            get
            {
                if (projects == null) projects = GenerateProjects();
                return projects;
            }
        }
        public Team[] Teams
        {
            get
            {
                if (teams == null) teams = GenerateTeam();
                return teams;
            }
        }

        // METHODS
        public void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(Users);
            modelBuilder.Entity<Project>().HasData(Projects);
            modelBuilder.Entity<Task>().HasData(Tasks);
            modelBuilder.Entity<Team>().HasData(Teams);
        }

        // GENERATORS
        private User[] GenerateUsers()
        {
            User[] users = new User[USERS_AMOUNT];
            for (int i = 0; i < USERS_AMOUNT; ++i)
            {
                users[i] = new User { Id = i + 1, FirstName = $"Name {i}", LastName = "LastName", Email = "email@mail.com", Birthday = Date.Now, RegisteredAt = Date.Now.AddMonths(Random.Next(10)), TeamId = Random.Next(TEAMS_AMOUNT - 1) };
            }
            users[USERS_AMOUNT - 1].Birthday = Date.Now.AddYears(-12);
            users[USERS_AMOUNT - 1].TeamId = TEAMS_AMOUNT;// last team, single user
            return users;
        }
        private Project[] GenerateProjects()
        {
            Project[] projects = new Project[PROJECTS_AMOUNT];
            for (int i = 0; i < PROJECTS_AMOUNT; ++i)
            {
                int authorId = Random.Next(USERS_AMOUNT);
                string projectName = string.Format(PROJECT_NAME_FORMAT, i);
                projects[i] = new Project { Id = i + 1, Name = projectName, CreatedAt = Date.Now, Deadline = Date.Now.AddMonths(Random.Next(5)), Description = "Description", AuthorId = authorId + 1 };

                ProjectAuthor[i] = authorId;
                ProjectTaskAmountPerUsers[authorId][projectName] = 0;
            }
            return projects;
        }
        private Task[] GenerateTasks()
        {
            Task[] tasks = new Task[TASKS_AMOUNT];
            for (int i = 0; i < TASKS_AMOUNT; ++i)
            {
                int projectId = Random.Next(PROJECTS_AMOUNT);
                int performerId = Random.Next(USERS_AMOUNT);

                string projectName = string.Format(PROJECT_NAME_FORMAT, projectId);

                tasks[i] = new Task { Id = i + 1, Name = $"Task {i}", Description = "Description", CreatedAt = Date.Now, FinishedAt = Date.Now.AddMonths(Random.Next(25)), PerformerId = performerId + 1, ProjectId = projectId + 1 };


                ++ProjectTaskAmountPerUsers[ProjectAuthor[projectId]][projectName];
                UsersTask[performerId].Add(tasks[i]);
            }
            return tasks;
        }

        private Team[] GenerateTeam()
        {
            Team[] teams = new Team[TEAMS_AMOUNT];
            for (int i = 0; i < TEAMS_AMOUNT; ++i)
            {
                teams[i] = new Team { Id = i + 1, Name = $"Team {i}" };
            }
            return teams;
        }
    }
}
