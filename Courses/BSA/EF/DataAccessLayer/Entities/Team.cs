using System.Collections.Generic;

namespace DataAccessLayer.Entities
{
    public class Team : EntityBase
    {
        [Newtonsoft.Json.JsonProperty("name")]
        public string Name { get; set; }
        [Newtonsoft.Json.JsonProperty("created_at")]
        public System.DateTime CreatedAt { get; set; }

        public ICollection<Project> Projects { get; set; } = new List<Project>();
        public ICollection<User> Users { get; set; } = new List<User>();

        public override string ToString()
        {
            return $"{Id} {Name}";
        }
    }
}
