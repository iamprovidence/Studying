namespace Core.DataTransferObjects.Project
{
    public class CreateProjectDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public System.DateTime Deadline { get; set; }
        
        public int AuthorId { get; set; }
        public int? TeamId { get; set; }
    }
}
