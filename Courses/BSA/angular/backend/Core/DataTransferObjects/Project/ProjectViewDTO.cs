namespace Core.DataTransferObjects.Project
{
    public class ProjectViewDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public System.DateTime CreatedAt { get; set; }
        public System.DateTime Deadline { get; set; }

        public int AuthorId { get; set; }
        public string AuthorName { get; set; }

        public int? TeamId { get; set; }
        public string TeamName { get; set; }
    }
}
