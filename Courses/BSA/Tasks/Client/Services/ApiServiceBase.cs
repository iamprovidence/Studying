using BusinessLayer.Commands;

namespace Client.Services
{
    public abstract class ApiServiceBase
    {
        // FIELDS
        protected System.Net.Http.HttpClient client;

        // CONSTRUCTORS
        public ApiServiceBase(System.Net.Http.HttpClient client)
        {
            this.client = client ?? throw new System.ArgumentNullException(nameof(client));
        }

        // METHODS
        public string GenerateUrl(string uri)
        {
            return string.Format(ServiceManagers.RestApiServiceManager.URL_FORMAT, uri);
        }
        public CommandResponse GenerateResponse(System.Net.Http.HttpResponseMessage responseMessage)
        {
            if (responseMessage.IsSuccessStatusCode)
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<CommandResponse>(responseMessage.Content.ReadAsStringAsync().Result);
            }
            else
            {
                return new CommandResponse
                {
                    IsSucessed = false,
                    Message = "Could not get response"
                };
            }
        }

    }
}
