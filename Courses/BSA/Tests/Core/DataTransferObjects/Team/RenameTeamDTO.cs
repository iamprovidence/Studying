using System.ComponentModel.DataAnnotations;

namespace Core.DataTransferObjects.Team
{
    public class RenameTeamDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
