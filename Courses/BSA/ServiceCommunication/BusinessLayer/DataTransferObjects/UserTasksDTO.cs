namespace BusinessLayer.DataTransferObjects
{
    public class UserTasksDTO
    {
        public DataAccessLayer.Entities.User User { get; set; }
        public System.Collections.Generic.IEnumerable<DataAccessLayer.Entities.Task> Tasks { get; set; }
    }
}
