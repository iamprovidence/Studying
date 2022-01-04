namespace Core.DataTransferObjects.User
{
    public class UpdateUserDTO
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
        public System.DateTime Birthday { get; set; }

        public int? TeamId { get; set; }
    }
}
