using System.Net.Http;
using System.Collections.Generic;

using DataAccessLayer.Entities;

using Client.ServiceManagers;

using Newtonsoft.Json;

using BusinessLayer.DataTransferObjects;

using Core.DataTransferObjects.User;


namespace Client.Services
{
    public class UserService : Interfaces.IUserService
    {
        // FIELDS
        private HttpClient client;

        // CONSTRUCTORS
        public UserService(HttpClient client)
        {
            this.client = client ?? throw new System.ArgumentNullException(nameof(client));
        }

        // METHODS
        public int CountTask(int userId, int projectId)
        {
            throw new System.NotImplementedException();
        }

        public User Get(int id)
        {
            string url = string.Format(RestApiServiceManager.URL_FORMAT, $@"api/users/{id}");

            return JsonConvert.DeserializeObject<User>(client.GetStringAsync(url).Result);
        }

        public IEnumerable<User> Get()
        {
            string url = string.Format(RestApiServiceManager.URL_FORMAT, $@"api/users");

            return JsonConvert.DeserializeObject<IEnumerable<User>>(client.GetStringAsync(url).Result);
        }

        public IEnumerable<UserTasksDTO> GetOrderedUsersWithTasks()
        {
            string url = string.Format(RestApiServiceManager.URL_FORMAT, $@"api/users/ordered");

            return JsonConvert.DeserializeObject<IEnumerable<UserTasksDTO>>(client.GetStringAsync(url).Result);
        }

        public bool Delete(int id)
        {
            string url = string.Format(RestApiServiceManager.URL_FORMAT, $@"api/users/{id}");

            return client.DeleteAsync(url).Result.IsSuccessStatusCode;
        }

        public bool Create(CreateUserDTO createUserDTO)
        {
            if (createUserDTO == null) throw new System.ArgumentNullException(nameof(createUserDTO));

            string url = string.Format(RestApiServiceManager.URL_FORMAT, $@"api/users");
            StringContent content = new StringContent(JsonConvert.SerializeObject(createUserDTO), System.Text.Encoding.UTF8, "application/json");

            return client.PostAsync(url, content).Result.IsSuccessStatusCode;
        }
    }
}
