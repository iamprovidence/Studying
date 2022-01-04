namespace DataAccessLayer.Entities
{
    public class Project : EntityBase
    {
        [Newtonsoft.Json.JsonProperty("name")]
        public string Name { get; set; }
        [Newtonsoft.Json.JsonProperty("description")]
        public string Description { get; set; }
        [Newtonsoft.Json.JsonProperty("created_at")]
        public System.DateTime CreatedAt { get; set; }
        [Newtonsoft.Json.JsonProperty("deadline")]
        public System.DateTime Deadline { get; set; }

        [Newtonsoft.Json.JsonProperty("author_id")]
        public int AuthorId { get; set; }
        [Newtonsoft.Json.JsonProperty("team_id")]
        public int? TeamId { get; set; }

        public override string ToString()
        {
            return $"{Id} - {Name}";
        }
    }
}
