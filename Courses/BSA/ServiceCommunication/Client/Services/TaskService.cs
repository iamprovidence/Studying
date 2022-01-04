using Client.ServiceManagers;

using DataAccessLayer.Entities;

using Newtonsoft.Json;

using System.Net.Http;
using System.Collections.Generic;

namespace Client.Services
{
    public class TaskService : Interfaces.ITaskService
    {
        // FIELDS
        private HttpClient client;

        // CONSTRUCTORS
        public TaskService(HttpClient client)
        {
            this.client = client ?? throw new System.ArgumentNullException(nameof(client));
        }

        // METHODS
        public int CountTask(int userId, int projectId)
        {
            string url = string.Format(RestApiServiceManager.URL_FORMAT, $@"api/tasks/count/?userId={userId}&projectId={projectId}");

            return JsonConvert.DeserializeObject<int>(client.GetStringAsync(url).Result);
        }

        public Task Get(int id)
        {
            string url = string.Format(RestApiServiceManager.URL_FORMAT, $@"api/tasks/{id}");

            return JsonConvert.DeserializeObject<Task>(client.GetStringAsync(url).Result);
        }

        public IEnumerable<Task> Get()
        {
            string url = string.Format(RestApiServiceManager.URL_FORMAT, $@"api/tasks/");

            return JsonConvert.DeserializeObject<IEnumerable<Task>>(client.GetStringAsync(url).Result);
        }

        public IEnumerable<Task> GetFinishedTasks(int userId)
        {
            string url = string.Format(RestApiServiceManager.URL_FORMAT, $@"api/tasks/finished/?userId={userId}");

            return JsonConvert.DeserializeObject<IEnumerable<Task>>(client.GetStringAsync(url).Result);
        }

        public Task GetLongestTask(int userId)
        {
            string url = string.Format(RestApiServiceManager.URL_FORMAT, $@"api/tasks/longest_period/?userId={userId}");

            return JsonConvert.DeserializeObject<Task>(client.GetStringAsync(url).Result);
        }

        public Task GetTaskWithLongestDescription(int projectId)
        {
            string url = string.Format(RestApiServiceManager.URL_FORMAT, $@"api/tasks/longest_description/?projectId={projectId}");

            return JsonConvert.DeserializeObject<Task>(client.GetStringAsync(url).Result);
        }

        public Task GetTaskWithShortestName(int projectId)
        {
            string url = string.Format(RestApiServiceManager.URL_FORMAT, $@"api/tasks/shortest_name/?projectId={projectId}");

            return JsonConvert.DeserializeObject<Task>(client.GetStringAsync(url).Result);
        }

        public IEnumerable<Task> GetTasksWithShortName(int userId)
        {
            string url = string.Format(RestApiServiceManager.URL_FORMAT, $@"api/tasks/short_name/?userId={userId}");

            return JsonConvert.DeserializeObject<IEnumerable<Task>>(client.GetStringAsync(url).Result);
        }

        public int UnfinishedTaskAmount(int userId)
        {
            string url = string.Format(RestApiServiceManager.URL_FORMAT, $@"api/tasks/unfinished/?userId={userId}");

            return JsonConvert.DeserializeObject<int>(client.GetStringAsync(url).Result);
        }

        public bool DeleteTask(int id)
        {
            string url = string.Format(RestApiServiceManager.URL_FORMAT, $@"api/tasks/{id}");

            return client.DeleteAsync(url).Result.IsSuccessStatusCode;
        }

        public bool CreateTask(Core.DataTransferObjects.Task.CreateTaskDTO createTaskDto)
        {
            if (createTaskDto == null) throw new System.ArgumentNullException(nameof(createTaskDto));

            string url = string.Format(RestApiServiceManager.URL_FORMAT, $@"api/tasks");
            StringContent content = new StringContent(JsonConvert.SerializeObject(createTaskDto), System.Text.Encoding.UTF8, "application/json");

            return client.PostAsync(url, content).Result.IsSuccessStatusCode;
        }
    }
}
