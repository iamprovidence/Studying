namespace DataAccessLayer.Entities
{
    public class Team : EntityBase
    {
        [Newtonsoft.Json.JsonProperty("name")]
        public string Name { get; set; }
        [Newtonsoft.Json.JsonProperty("created_at")]
        public System.DateTime CreatedAt { get; set; }

        public override string ToString()
        {
            return $"{Id} {Name}";
        }
    }
}
