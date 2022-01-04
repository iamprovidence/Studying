namespace DataAccess.Models
{
    public class TaskState
    {
        [Newtonsoft.Json.JsonProperty("id")]
        public int Id { get; set; }
        [Newtonsoft.Json.JsonProperty("value")]
        public string Value { get; set; }
    }
}
