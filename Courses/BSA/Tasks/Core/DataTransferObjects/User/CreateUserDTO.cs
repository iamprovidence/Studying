using System.ComponentModel.DataAnnotations;

namespace Core.DataTransferObjects.User
{
    public class CreateUserDTO
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }
        [Required]
        public System.DateTime Birthday { get; set; }
        [Required]
        public System.DateTime RegisteredAt { get; set; }
        
        [Required]
        public int TeamId { get; set; }
    }
}
