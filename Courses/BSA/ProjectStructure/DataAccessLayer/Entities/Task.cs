namespace DataAccessLayer.Entities
{
    public class Task : EntityBase
    {
        [Newtonsoft.Json.JsonProperty("name")]
        public string Name { get; set; }
        [Newtonsoft.Json.JsonProperty("description")]
        public string Description { get; set; }
        [Newtonsoft.Json.JsonProperty("created_at")]
        public System.DateTime CreatedAt { get; set; }
        [Newtonsoft.Json.JsonProperty("finished_at")]
        public System.DateTime FinishedAt { get; set; }

        [Newtonsoft.Json.JsonProperty("project_id")]
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        [Newtonsoft.Json.JsonProperty("performer_id")]
        public int? PerformerId { get; set; }
        public User Performer { get; set; }
        [Newtonsoft.Json.JsonProperty("state")]
        public int? TaskStateId { get; set; }
        public TaskState TaskState { get; set; }

        public override string ToString()
        {
            return $"{Id} - {Name} {CreatedAt.ToShortDateString()} - {FinishedAt.ToShortDateString()}";
        }
    }
}
