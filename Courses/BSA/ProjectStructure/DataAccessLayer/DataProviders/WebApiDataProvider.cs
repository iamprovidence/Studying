using DataAccessLayer.Entities;

using Newtonsoft.Json;

using System.Collections.Generic;

namespace DataAccessLayer.DataProviders
{
    public class WebApiDataProvider : Interfaces.IDataProvider, System.IDisposable
    {
        // CONSTS
        static readonly string URL_FORMAT = "https://bsa2019.azurewebsites.net/api/{0}";

        // FIELDS
        System.Net.Http.HttpClient httpClient;

        User[] users;
        Task[] tasks;
        Project[] projects;
        Team[] teams;
        TaskState[] taskStates;

        // CONSTRUCTORS
        public WebApiDataProvider()
        {
            httpClient = new System.Net.Http.HttpClient();

            users = null;
            tasks = null;
            projects = null;
            teams = null;
            taskStates = null;
        }
        public void Dispose()
        {
            httpClient.Dispose();
        }

        // PROPERTIES
        public IEnumerable<User> Users
        {
            get
            {
                if (users == null)
                {
                    users = JsonConvert.DeserializeObject<User[]>
                        (httpClient.GetStringAsync(string.Format(URL_FORMAT, "Users")).Result);
                }
                return users;
            }
        }
        public IEnumerable<Task> Tasks
        {
            get
            {
                if (tasks == null)
                {
                    tasks = JsonConvert.DeserializeObject<Task[]>
                       (httpClient.GetStringAsync(string.Format(URL_FORMAT, "Tasks")).Result);
                }
                return tasks;
            }
        }
        public IEnumerable<Project> Projects
        {
            get
            {
                if (projects == null)
                {
                    projects = JsonConvert.DeserializeObject<Project[]>
                       (httpClient.GetStringAsync(string.Format(URL_FORMAT, "Projects")).Result);
                }
                return projects;
            }
        }
        public IEnumerable<Team> Teams
        {
            get
            {
                if (teams == null)
                {
                    teams = JsonConvert.DeserializeObject<Team[]>
                        (httpClient.GetStringAsync(string.Format(URL_FORMAT, "Teams")).Result);
                }
                return teams;
            }
        }
        public IEnumerable<TaskState> TaskStates
        {
            get
            {
                if (taskStates == null)
                {
                    taskStates = JsonConvert.DeserializeObject<TaskState[]>
                        (httpClient.GetStringAsync(string.Format(URL_FORMAT, "TaskStates")).Result);
                }
                return taskStates;
            }
        }

    }
}
