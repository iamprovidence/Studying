using System.Collections.Generic;

namespace DataAccessLayer.Entities
{
    public class TaskState : EntityBase
    {
        [Newtonsoft.Json.JsonProperty("value")]
        public string Value { get; set; }

        public ICollection<Task> Tasks { get; set; } = new List<Task>();
    }
}
