using System.Net.Http;
using System.Threading.Tasks;
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

        public async Task<User> GetAsync(int id)
        {
            string url = GenerateUrl($@"api/users/{id}");

            return JsonConvert.DeserializeObject<User>(await client.GetStringAsync(url));
        }

        public async Task<IEnumerable<User>> GetAsync()
        {
            string url = GenerateUrl($@"api/users");

            return JsonConvert.DeserializeObject<IEnumerable<User>>(await client.GetStringAsync(url));
        }

        public async Task<IEnumerable<UserTasksDTO>> GetOrderedUsersWithTasksAsync()
        {
            string url = GenerateUrl($@"api/users/ordered");

            return JsonConvert.DeserializeObject<IEnumerable<UserTasksDTO>>(await client.GetStringAsync(url));
        }

        public async Task<CommandResponse> DeleteAsync(int id)
        {
            string url = GenerateUrl($@"api/users/{id}");
           
            return GenerateResponse(await client.DeleteAsync(url));
        }

        public async Task<CommandResponse> CreateAsync(CreateUserDTO createUserDTO)
        {
            if (createUserDTO == null) throw new System.ArgumentNullException(nameof(createUserDTO));

            string url = GenerateUrl($@"api/users");
            StringContent content = new StringContent(JsonConvert.SerializeObject(createUserDTO), System.Text.Encoding.UTF8, "application/json");

            return GenerateResponse(await client.PostAsync(url, content));
        }
    }
}
