using System.ComponentModel.DataAnnotations;

namespace Core.DataTransferObjects.Team
{
    public class CreateTeamDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public System.DateTime CreatedAt { get; set; }
    }
}
