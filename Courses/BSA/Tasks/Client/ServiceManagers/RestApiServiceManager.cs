using System;

using Client.Services;
using Client.Interfaces;

namespace Client.ServiceManagers
{
    public class RestApiServiceManager : IServiceManager, IDisposable
    {
        // CONST
        public static readonly string URL_FORMAT = "https://localhost:5001/{0}";

        // FIELDS
        System.Net.Http.HttpClient client;

        ITaskService taskService;
        IProjectService projectService;
        ITeamService teamService;
        IUserService userService;
        IMessageService messageService;
        IFileService fileService;

        // CONSTRUCTORS
        public RestApiServiceManager()
        {
            client = new System.Net.Http.HttpClient();

            taskService = null;
            projectService = null;
            teamService = null;
            userService = null;
            messageService = null;
        }
        public void Dispose()
        {
            client.Dispose();
            (messageService as IDisposable)?.Dispose();
        }

        // PROPERTIES
        public ITaskService TaskService
        {
            get
            {
                if (taskService == null)
                {
                    taskService = new TaskService(client);
                }
                return taskService;
            }
        }
        public IProjectService ProjectService
        {
            get
            {
                if (projectService == null)
                {
                    projectService = new ProjectService(client);
                }
                return projectService;
            }
        }
        public ITeamService TeamService
        {
            get
            {
                if (teamService == null)
                {
                    teamService = new TeamService(client);
                }
                return teamService;
            }
        }

        public IUserService UserService
        {
            get
            {
                if (userService == null)
                {
                    userService = new UserService(client);
                }
                return userService;
            }
        }

        public IMessageService MessageService
        {
            get
            {
                if (messageService == null)
                {
                    messageService = new HubMessageService(string.Format(URL_FORMAT, "message"));
                }
                return messageService;
            }
        }

        public IFileService FileService
        {
            get
            {
                if (fileService == null)
                {
                    fileService = new FileService(client);
                }
                return fileService;
            }
        }
    }
}
