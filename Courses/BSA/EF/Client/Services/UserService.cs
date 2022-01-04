using System.Net.Http;
using System.Collections.Generic;

using DataAccessLayer.Entities;

using Newtonsoft.Json;

using BusinessLayer.Commands;
using BusinessLayer.DataTransferObjects;

using Core.DataTransferObjects.User;


namespace Client.Services
{
    public class UserService : ApiServiceBase, Interfaces.IUserService
    {
        // CONSTRUCTORS
        public UserService(HttpClient client)
            : base(client) { }

        // METHODS

        public User Get(int id)
        {
            string url = GenerateUrl($@"api/users/{id}");

            return JsonConvert.DeserializeObject<User>(client.GetStringAsync(url).Result);
        }

        public IEnumerable<User> Get()
        {
            string url = GenerateUrl($@"api/users");

            return JsonConvert.DeserializeObject<IEnumerable<User>>(client.GetStringAsync(url).Result);
        }

        public IEnumerable<UserTasksDTO> GetOrderedUsersWithTasks()
        {
            string url = GenerateUrl($@"api/users/ordered");

            return JsonConvert.DeserializeObject<IEnumerable<UserTasksDTO>>(client.GetStringAsync(url).Result);
        }

        public CommandResponse Delete(int id)
        {
            string url = GenerateUrl($@"api/users/{id}");
           
            return GenerateResponse(client.DeleteAsync(url).Result);
        }

        public CommandResponse Create(CreateUserDTO createUserDTO)
        {
            if (createUserDTO == null) throw new System.ArgumentNullException(nameof(createUserDTO));

            string url = GenerateUrl($@"api/users");
            StringContent content = new StringContent(JsonConvert.SerializeObject(createUserDTO), System.Text.Encoding.UTF8, "application/json");

            return GenerateResponse(client.PostAsync(url, content).Result);
        }
    }
}
