namespace Core.DataTransferObjects.Task
{
    public class TaskViewDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public System.DateTime CreatedAt { get; set; }
        public System.DateTime FinishedAt { get; set; }

        public int ProjectId { get; set; }
        public string ProjectName { get; set; }

        public int? PerformerId { get; set; }
        public string PerformerName { get; set; }

        public int TaskStateId { get; set; }
        public string StateName { get; set; }
    }
}
