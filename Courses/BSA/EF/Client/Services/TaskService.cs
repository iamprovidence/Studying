using DataAccessLayer.Entities;

using Newtonsoft.Json;

using System.Net.Http;
using System.Collections.Generic;

using BusinessLayer.Commands;

namespace Client.Services
{
    public class TaskService : ApiServiceBase, Interfaces.ITaskService
    {
        // CONSTRUCTORS
        public TaskService(HttpClient client)
            : base(client) { }

        // METHODS
        public int CountTask(int userId, int projectId)
        {
            string url = GenerateUrl($@"api/tasks/count/?userId={userId}&projectId={projectId}");

            return JsonConvert.DeserializeObject<int>(client.GetStringAsync(url).Result);
        }

        public Task Get(int id)
        {
            string url = GenerateUrl($@"api/tasks/{id}");

            return JsonConvert.DeserializeObject<Task>(client.GetStringAsync(url).Result);
        }

        public IEnumerable<Task> Get()
        {
            string url = GenerateUrl($@"api/tasks/");

            return JsonConvert.DeserializeObject<IEnumerable<Task>>(client.GetStringAsync(url).Result);
        }

        public IEnumerable<Task> GetFinishedTasks(int userId)
        {
            string url = GenerateUrl($@"api/tasks/finished/?userId={userId}");

            return JsonConvert.DeserializeObject<IEnumerable<Task>>(client.GetStringAsync(url).Result);
        }

        public Task GetLongestTask(int userId)
        {
            string url = GenerateUrl($@"api/tasks/longest_period/?userId={userId}");

            return JsonConvert.DeserializeObject<Task>(client.GetStringAsync(url).Result);
        }

        public Task GetTaskWithLongestDescription(int projectId)
        {
            string url = GenerateUrl($@"api/tasks/longest_description/?projectId={projectId}");

            return JsonConvert.DeserializeObject<Task>(client.GetStringAsync(url).Result);
        }

        public Task GetTaskWithShortestName(int projectId)
        {
            string url = GenerateUrl($@"api/tasks/shortest_name/?projectId={projectId}");

            return JsonConvert.DeserializeObject<Task>(client.GetStringAsync(url).Result);
        }

        public IEnumerable<Task> GetTasksWithShortName(int userId)
        {
            string url = GenerateUrl($@"api/tasks/short_name/?userId={userId}");

            return JsonConvert.DeserializeObject<IEnumerable<Task>>(client.GetStringAsync(url).Result);
        }

        public int UnfinishedTaskAmount(int userId)
        {
            string url = GenerateUrl($@"api/tasks/unfinished/?userId={userId}");

            return JsonConvert.DeserializeObject<int>(client.GetStringAsync(url).Result);
        }

        public CommandResponse DeleteTask(int id)
        {
            string url = GenerateUrl($@"api/tasks/{id}");
            
            return GenerateResponse(client.DeleteAsync(url).Result);
        }

        public CommandResponse CreateTask(Core.DataTransferObjects.Task.CreateTaskDTO createTaskDto)
        {
            if (createTaskDto == null) throw new System.ArgumentNullException(nameof(createTaskDto));

            string url = GenerateUrl($@"api/tasks");
            StringContent content = new StringContent(JsonConvert.SerializeObject(createTaskDto), System.Text.Encoding.UTF8, "application/json");

            // result
            return GenerateResponse(client.PostAsync(url, content).Result);
        }
    }
}
