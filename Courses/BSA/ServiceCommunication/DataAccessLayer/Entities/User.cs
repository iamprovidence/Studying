namespace DataAccessLayer.Entities
{
    public class User : EntityBase
    {
        [Newtonsoft.Json.JsonProperty("first_name")]
        public string FirstName { get; set; }
        [Newtonsoft.Json.JsonProperty("last_name")]
        public string LastName { get; set; }
        [Newtonsoft.Json.JsonProperty("email")]
        public string Email { get; set; }
        [Newtonsoft.Json.JsonProperty("birthday")]
        public System.DateTime Birthday { get; set; }
        [Newtonsoft.Json.JsonProperty("registered_at")]
        public System.DateTime RegisteredAt { get; set; }


        [Newtonsoft.Json.JsonProperty("team_id")]
        public int? TeamId { get; set; }

        public override string ToString()
        {
            return $"{Id} - {FirstName} {LastName} {Email} {Birthday}";
        }
    }
}
