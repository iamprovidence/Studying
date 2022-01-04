using System.ComponentModel.DataAnnotations;

namespace Core.DataTransferObjects.Task
{
    public class CreateTaskDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public System.DateTime CreatedAt { get; set; }
        [Required]
        public System.DateTime FinishedAt { get; set; }

        [Required]
        public int ProjectId { get; set; }
        [Required]
        public int PerformerId { get; set; }
    }
}
