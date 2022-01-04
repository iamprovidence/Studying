using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

using DataAccessLayer.Entities;

using Newtonsoft.Json;

using Core.DataTransferObjects.Project;

using BusinessLayer.Commands;

namespace Client.Services
{
    public class ProjectService : ApiServiceBase, Interfaces.IProjectService
    {
        // CONSTRUCTORS
        public ProjectService(HttpClient client)
            : base(client) { }

        // METHODS
        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            string url = GenerateUrl($@"api/projects");

            return JsonConvert.DeserializeObject<IEnumerable<Project>>(await client.GetStringAsync(url));
        }

        public async Task<Project> GetProjectAsync(int id)
        {
            string url = GenerateUrl($@"api/projects/{id}");

            return JsonConvert.DeserializeObject<Project>(await client.GetStringAsync(url));
        }

        public async Task<Project> GetLastProjectAsync(int userId)
        {
            string url = GenerateUrl($@"api/projects/last_project/?userId={userId}");

            return JsonConvert.DeserializeObject<Project>(await client.GetStringAsync(url));
        }

        public async Task<IDictionary<string, int>> GetTasksAmountPerProjectAsync(int userId)
        {
            string url = GenerateUrl($@"api/projects/task_per_project/?userId={userId}");

            return JsonConvert.DeserializeObject<IDictionary<string, int>>(await client.GetStringAsync(url));
        }

        public async Task<CommandResponse> DeleteAsync(int id)
        {
            string url = GenerateUrl($@"api/projects/{id}");

            return GenerateResponse(await client.DeleteAsync(url));
        }

        public async Task<CommandResponse> CreateAsync(CreateProjectDTO createProjectDTO)
        {
            if (createProjectDTO == null) throw new System.ArgumentNullException(nameof(createProjectDTO));

            string url = GenerateUrl($@"api/projects");
            StringContent content = new StringContent(JsonConvert.SerializeObject(createProjectDTO), System.Text.Encoding.UTF8, "application/json");

            return GenerateResponse(await client.PostAsync(url, content));
        }
    }
}
