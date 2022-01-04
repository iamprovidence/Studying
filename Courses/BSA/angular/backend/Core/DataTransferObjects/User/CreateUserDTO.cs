namespace Core.DataTransferObjects.User
{
    public class CreateUserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        public string Email { get; set; }
        public System.DateTime Birthday { get; set; }
        
        public int? TeamId { get; set; }
    }
}
