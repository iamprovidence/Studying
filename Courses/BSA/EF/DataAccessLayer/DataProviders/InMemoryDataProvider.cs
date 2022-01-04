using System.Collections.Generic;

using DataAccessLayer.Entities;

namespace DataAccessLayer.DataProviders
{
    public class InMemoryDataProvider : Interfaces.IDataProvider
    {
        // FIELDS
        User[] users;
        Task[] tasks;
        Project[] projects;
        Team[] teams;
        TaskState[] taskStates;

        System.Random random;

        // CONSTRUCTORS
        public InMemoryDataProvider()
        {
            users = null;
            tasks = null;
            projects = null;
            teams = null;
            taskStates = null;

            random = new System.Random();
        }


        // PROPERTIES
        public IEnumerable<User> Users
        {
            get
            {
                if (users == null) users = BuildUsers(50);
                return users;
            }
        }

        public IEnumerable<Task> Tasks
        {
            get
            {
                if (tasks == null) tasks = BuildTasks(200);
                return tasks;
            }
        }

        public IEnumerable<Project> Projects
        {
            get
            {
                if (projects == null) projects = BuildProjects(100);
                return projects;
            }
        }

        public IEnumerable<Team> Teams
        {
            get
            {
                if (teams == null) teams = BuildTeams(30);
                return teams;
            }
        }

        public IEnumerable<TaskState> TaskStates
        {
            get
            {
                if (taskStates == null) taskStates = BuildTaskStates();
                return taskStates;
            }
        }

        // METHODS
        public string RandomString(int length)
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder(length);

            for (int i = 0; i < length; ++i)
            {
                stringBuilder.Append((char)random.Next(97, 127));
            }
            return stringBuilder.ToString();
        }

        public User[] BuildUsers(int amount)
        {
            User[] users = new User[amount];
            for (int i = 0; i < amount; ++i)
            {
                users[i] = new User
                {
                    Id = i + 1,

                    FirstName = RandomString(10),
                    LastName = RandomString(10),
                    Email = RandomString(10),

                    TeamId = random.Next(50),

                    Birthday = System.DateTime.Now.AddMonths(-random.Next(5)),
                    RegisteredAt = System.DateTime.Now.AddMonths(random.Next(5)),

                };
            }
            return users;
        }
        public Task[] BuildTasks(int amount)
        {
            Task[] tasks = new Task[amount];
            for (int i = 0; i < amount; ++i)
            {
                tasks[i] = new Task
                {
                    Id = i + 1,

                    Name = RandomString(15),
                    Description = RandomString(25),

                    CreatedAt = System.DateTime.Now.AddMonths(-random.Next(5)),
                    FinishedAt = System.DateTime.Now.AddMonths(random.Next(5)),

                    PerformerId = random.Next(15),
                    ProjectId = random.Next(50),
                    TaskStateId = random.Next(4)
                };
            }
            return tasks;
        }
        public Project[] BuildProjects(int amount)
        {
            Project[] projects = new Project[amount];
            for (int i = 0; i < amount; ++i)
            {
                projects[i] = new Project
                {
                    Id = i + 1,

                    Name = RandomString(15),
                    Description = RandomString(25),

                    CreatedAt = System.DateTime.Now.AddMonths(-random.Next(5)),
                    Deadline = System.DateTime.Now.AddMonths(random.Next(5)),

                    AuthorId = random.Next(10),
                    TeamId = random.Next(25)
                };
            }
            return projects;
        }
        public Team[] BuildTeams(int amount)
        {
            Team[] teams = new Team[amount];
            for (int i = 0; i < amount; ++i)
            {
                teams[i] = new Team
                {
                    Id = i + 1,

                    Name = RandomString(15),

                    CreatedAt = System.DateTime.Now.AddMonths(-random.Next(5)),
                };
            }
            return teams;
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
