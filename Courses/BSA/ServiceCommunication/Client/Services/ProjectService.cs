using System.Net.Http;
using System.Collections.Generic;

using DataAccessLayer.Entities;

using Client.ServiceManagers;

using Newtonsoft.Json;

using Core.DataTransferObjects.Project;


namespace Client.Services
{
    public class ProjectService : Interfaces.IProjectService
    {
        // FIELDS
        private HttpClient client;

        // CONSTRUCTORS
        public ProjectService(HttpClient client)
        {
            this.client = client ?? throw new System.ArgumentNullException(nameof(client));
        }

        // METHODS
        public IEnumerable<Project> GetAll()
        {
            string url = string.Format(RestApiServiceManager.URL_FORMAT, $@"api/projects");

            return JsonConvert.DeserializeObject<IEnumerable<Project>>(client.GetStringAsync(url).Result);
        }

        public Project GetProject(int id)
        {
            string url = string.Format(RestApiServiceManager.URL_FORMAT, $@"api/projects/{id}");

            return JsonConvert.DeserializeObject<Project>(client.GetStringAsync(url).Result);
        }

        public Project GetLastProject(int userId)
        {
            string url = string.Format(RestApiServiceManager.URL_FORMAT, $@"api/projects/last_project/?userId={userId}");

            return JsonConvert.DeserializeObject<Project>(client.GetStringAsync(url).Result);
        }

        public IDictionary<string, int> GetTasksAmountPerProject(int userId)
        {
            string url = string.Format(RestApiServiceManager.URL_FORMAT, $@"api/projects/task_per_project/?userId={userId}");

            return JsonConvert.DeserializeObject<IDictionary<string, int>>(client.GetStringAsync(url).Result);
        }

        public bool Delete(int id)
        {
            string url = string.Format(RestApiServiceManager.URL_FORMAT, $@"api/projects/{id}");

            return client.DeleteAsync(url).Result.IsSuccessStatusCode;
        }

        public bool Create(CreateProjectDTO createProjectDTO)
        {
            if (createProjectDTO == null) throw new System.ArgumentNullException(nameof(createProjectDTO));

            string url = string.Format(RestApiServiceManager.URL_FORMAT, $@"api/projects");
            StringContent content = new StringContent(JsonConvert.SerializeObject(createProjectDTO), System.Text.Encoding.UTF8, "application/json");

            return client.PostAsync(url, content).Result.IsSuccessStatusCode;
        }
    }
}
