using System.Collections.Generic;
using System.Net.Http;
using System.Text;

using Client.ServiceManagers;

using Core.DataTransferObjects.Team;

using DataAccessLayer.Entities;

using Newtonsoft.Json;

namespace Client.Services
{
    public class TeamService : Interfaces.ITeamService
    {
        // FIELDS
        private HttpClient client;

        // CONSTRUCTORS
        public TeamService(HttpClient client)
        {
            this.client = client ?? throw new System.ArgumentNullException(nameof(client));
        }
        
        // METHODS
        public IEnumerable<Team> Get()
        {
            string url = string.Format(RestApiServiceManager.URL_FORMAT, @"api/teams");

            return JsonConvert.DeserializeObject<IEnumerable<Team>>(client.GetStringAsync(url).Result);
        }

        public IEnumerable<BusinessLayer.DataTransferObjects.TeamUsersDTO> GetTeamByAgeLimit(int participantsAge = 9)
        {
            string url = string.Format(RestApiServiceManager.URL_FORMAT, $@"api/teams/limited/?participantAge={participantsAge}");

            return JsonConvert.DeserializeObject<IEnumerable<BusinessLayer.DataTransferObjects.TeamUsersDTO>>(client.GetStringAsync(url).Result);
        }

        public IDictionary<int, int> GetTeamUserAmount()
        {
            string url = string.Format(RestApiServiceManager.URL_FORMAT, $@"api/teams/participants_amount/");

            return JsonConvert.DeserializeObject<IDictionary<int, int>>(client.GetStringAsync(url).Result);
        }

        public bool RenameTeam(RenameTeamDTO value)
        {
            if (value == null) throw new System.ArgumentNullException(nameof(value));

            string url = string.Format(RestApiServiceManager.URL_FORMAT, @"api/teams");
            StringContent content = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");

            return client.PatchAsync(url, content).Result.IsSuccessStatusCode;
        }

        public bool DeleteTeam(int id)
        {
            string url = string.Format(RestApiServiceManager.URL_FORMAT, $@"api/teams/{id}");

            return client.DeleteAsync(url).Result.IsSuccessStatusCode;
        }

        public bool CreateTeam(CreateTeamDTO createTeamDTO)
        {
            if (createTeamDTO == null) throw new System.ArgumentNullException(nameof(createTeamDTO));

            string url = string.Format(RestApiServiceManager.URL_FORMAT, $@"api/teams");
            StringContent content = new StringContent(JsonConvert.SerializeObject(createTeamDTO), Encoding.UTF8, "application/json");

            return client.PostAsync(url, content).Result.IsSuccessStatusCode;
        }
    }
}
