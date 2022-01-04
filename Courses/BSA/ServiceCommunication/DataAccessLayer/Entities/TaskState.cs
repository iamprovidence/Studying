namespace DataAccessLayer.Entities
{
    public class TaskState : EntityBase
    {
        [Newtonsoft.Json.JsonProperty("value")]
        public string Value { get; set; }
    }
}
