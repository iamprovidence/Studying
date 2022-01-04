using Newtonsoft.Json;

using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

using BusinessLayer.Commands;

using TaskDb = DataAccessLayer.Entities.Task;

namespace Client.Services
{
    public class TaskService : ApiServiceBase, Interfaces.ITaskService
    {
        // CONSTRUCTORS
        public TaskService(HttpClient client)
            : base(client) { }

        // METHODS
        public async Task<int> CountTaskAsync(int userId, int projectId)
        {
            string url = GenerateUrl($@"api/tasks/count/?userId={userId}&projectId={projectId}");

            return JsonConvert.DeserializeObject<int>(await client.GetStringAsync(url));
        }

        public async Task<TaskDb> GetAsync(int id)
        {
            string url = GenerateUrl($@"api/tasks/{id}");

            return JsonConvert.DeserializeObject<TaskDb>(await client.GetStringAsync(url));
        }

        public async Task<IEnumerable<TaskDb>> GetAsync()
        {
            string url = GenerateUrl($@"api/tasks/");

            return JsonConvert.DeserializeObject<IEnumerable<TaskDb>>(await client.GetStringAsync(url));
        }

        public async Task<IEnumerable<TaskDb>> GetFinishedTasksAsync(int userId)
        {
            string url = GenerateUrl($@"api/tasks/finished/?userId={userId}");

            return JsonConvert.DeserializeObject<IEnumerable<TaskDb>>(await client.GetStringAsync(url));
        }

        public async Task<TaskDb> GetLongestTaskAsync(int userId)
        {
            string url = GenerateUrl($@"api/tasks/longest_period/?userId={userId}");

            return JsonConvert.DeserializeObject<TaskDb>(await client.GetStringAsync(url));
        }

        public async Task<TaskDb> GetTaskWithLongestDescriptionAsync(int projectId)
        {
            string url = GenerateUrl($@"api/tasks/longest_description/?projectId={projectId}");

            return JsonConvert.DeserializeObject<TaskDb>(await client.GetStringAsync(url));
        }

        public async Task<TaskDb> GetTaskWithShortestNameAsync(int projectId)
        {
            string url = GenerateUrl($@"api/tasks/shortest_name/?projectId={projectId}");

            return JsonConvert.DeserializeObject<TaskDb>(await client.GetStringAsync(url));
        }

        public async Task<IEnumerable<TaskDb>> GetTasksWithShortNameAsync(int userId)
        {
            string url = GenerateUrl($@"api/tasks/short_name/?userId={userId}");

            return JsonConvert.DeserializeObject<IEnumerable<TaskDb>>(await client.GetStringAsync(url));
        }

        public async Task<int> UnfinishedTaskAmountAsync(int userId)
        {
            string url = GenerateUrl($@"api/tasks/unfinished/?userId={userId}");

            return JsonConvert.DeserializeObject<int>(await client.GetStringAsync(url));
        }

        public async Task<CommandResponse> DeleteTaskAsync(int id)
        {
            string url = GenerateUrl($@"api/tasks/{id}");
            
            return GenerateResponse(await client.DeleteAsync(url));
        }

        public async Task<CommandResponse> CreateTaskAsync(Core.DataTransferObjects.Task.CreateTaskDTO createTaskDto)
        {
            if (createTaskDto == null) throw new System.ArgumentNullException(nameof(createTaskDto));

            string url = GenerateUrl($@"api/tasks");
            StringContent content = new StringContent(JsonConvert.SerializeObject(createTaskDto), System.Text.Encoding.UTF8, "application/json");

            // result
            return GenerateResponse(await client.PostAsync(url, content));
        }
    }
}
