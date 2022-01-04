using System.ComponentModel.DataAnnotations;

namespace Core.DataTransferObjects.Project
{
    public class CreateProjectDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public System.DateTime CreatedAt { get; set; }
        [Required]
        public System.DateTime Deadline { get; set; }

        [Required]
        public int AuthorId { get; set; }
        [Required]
        public int TeamId { get; set; }

    }
}
