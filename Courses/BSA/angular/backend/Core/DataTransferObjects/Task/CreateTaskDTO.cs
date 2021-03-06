namespace Core.DataTransferObjects.Task
{
    public class CreateTaskDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public System.DateTime FinishedAt { get; set; }
        
        public int ProjectId { get; set; }
        public int? PerformerId { get; set; }
        public int TaskStateId { get; set; }
    }
}
