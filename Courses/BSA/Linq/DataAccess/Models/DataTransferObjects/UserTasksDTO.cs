namespace DataAccess.Models.DataTransferObjects
{
    public class UserTasksDTO
    {
        public User User { get; set; }
        public System.Collections.Generic.IEnumerable<Task> Tasks { get; set; }
    }
}
