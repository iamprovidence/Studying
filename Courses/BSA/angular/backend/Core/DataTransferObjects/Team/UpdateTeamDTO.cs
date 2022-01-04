using System.ComponentModel.DataAnnotations;

namespace Core.DataTransferObjects.Team
{
    public class UpdateTeamDTO
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
    }
}
