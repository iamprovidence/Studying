namespace BusinessLayer.DataTransferObjects
{
    public class UserTasksDTO
    {
        public string UserName { get; set; }
        public System.Collections.Generic.IEnumerable<string> TaskNames { get; set; }
    }
}
