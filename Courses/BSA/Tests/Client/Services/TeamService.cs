using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

using BusinessLayer.Commands;

using Core.DataTransferObjects.Team;

using DataAccessLayer.Entities;

using Newtonsoft.Json;

namespace Client.Services
{
    public class TeamService : ApiServiceBase, Interfaces.ITeamService
    {
        // CONSTRUCTORS
        public TeamService(HttpClient client)
            : base(client) { }
        
        // METHODS
        public async Task<IEnumerable<Team>> GetAsync()
        {
            string url = GenerateUrl(@"api/teams");

            return JsonConvert.DeserializeObject<IEnumerable<Team>>(await client.GetStringAsync(url));
        }

        public async Task<IEnumerable<BusinessLayer.DataTransferObjects.TeamUsersDTO>> GetTeamByAgeLimitAsync(int participantsAge = 9)
        {
            string url = GenerateUrl($@"api/teams/limited/?participantAge={participantsAge}");

            return JsonConvert.DeserializeObject<IEnumerable<BusinessLayer.DataTransferObjects.TeamUsersDTO>>(await client.GetStringAsync(url));
        }

        public async Task<IDictionary<int, int>> GetTeamUserAmountAsync()
        {
            string url = GenerateUrl($@"api/teams/participants_amount/");

            return JsonConvert.DeserializeObject<IDictionary<int, int>>(await client.GetStringAsync(url));
        }

        public async Task<CommandResponse> RenameTeamAsync(RenameTeamDTO value)
        {
            if (value == null) throw new System.ArgumentNullException(nameof(value));

            string url = GenerateUrl(@"api/teams");
            StringContent content = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");

            // result
            return GenerateResponse(await client.PatchAsync(url, content));
        }

        public async Task<CommandResponse> DeleteTeamAsync(int id)
        {
            string url = GenerateUrl($@"api/teams/{id}");

            // result
            return GenerateResponse(await client.DeleteAsync(url));
        }

        public async Task<CommandResponse> CreateTeamAsync(CreateTeamDTO createTeamDTO)
        {
            if (createTeamDTO == null) throw new System.ArgumentNullException(nameof(createTeamDTO));

            string url =  GenerateUrl($@"api/teams");
            StringContent content = new StringContent(JsonConvert.SerializeObject(createTeamDTO), Encoding.UTF8, "application/json");

            // result
            return GenerateResponse(await client.PostAsync(url, content));
        }
    }
}
