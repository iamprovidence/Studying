using System.Net.Http;
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
        public IEnumerable<Project> GetAll()
        {
            string url = GenerateUrl($@"api/projects");

            return JsonConvert.DeserializeObject<IEnumerable<Project>>(client.GetStringAsync(url).Result);
        }

        public Project GetProject(int id)
        {
            string url = GenerateUrl($@"api/projects/{id}");

            return JsonConvert.DeserializeObject<Project>(client.GetStringAsync(url).Result);
        }

        public Project GetLastProject(int userId)
        {
            string url = GenerateUrl($@"api/projects/last_project/?userId={userId}");

            return JsonConvert.DeserializeObject<Project>(client.GetStringAsync(url).Result);
        }

        public IDictionary<string, int> GetTasksAmountPerProject(int userId)
        {
            string url = GenerateUrl($@"api/projects/task_per_project/?userId={userId}");

            return JsonConvert.DeserializeObject<IDictionary<string, int>>(client.GetStringAsync(url).Result);
        }

        public CommandResponse Delete(int id)
        {
            string url = GenerateUrl($@"api/projects/{id}");

            return GenerateResponse(client.DeleteAsync(url).Result);
        }

        public CommandResponse Create(CreateProjectDTO createProjectDTO)
        {
            if (createProjectDTO == null) throw new System.ArgumentNullException(nameof(createProjectDTO));

            string url = GenerateUrl($@"api/projects");
            StringContent content = new StringContent(JsonConvert.SerializeObject(createProjectDTO), System.Text.Encoding.UTF8, "application/json");

            return GenerateResponse(client.PostAsync(url, content).Result);
        }
    }
}
