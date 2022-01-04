namespace DataAccessLayer.Entities
{
    public abstract class EntityBase : Interfaces.IEntity
    {
        [Newtonsoft.Json.JsonProperty("id")]
        public int Id { get; set; }
    }
}