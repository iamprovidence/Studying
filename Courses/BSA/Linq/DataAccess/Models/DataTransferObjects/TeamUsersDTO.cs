namespace DataAccess.Models.DataTransferObjects
{
    public class TeamUsersDTO
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }

        public System.Collections.Generic.IEnumerable<User> Participants { get; set; }
    }
}
