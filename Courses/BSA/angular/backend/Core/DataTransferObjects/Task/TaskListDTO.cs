namespace Core.DataTransferObjects.Task
{
    public class TaskListDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string ShortDescription { get; set; }

        public int TaskStateId { get; set; }
    }
}
