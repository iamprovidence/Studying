namespace BusinessLayer.DataTransferObjects
{
    public class TeamUsersDTO
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }

        public System.Collections.Generic.IEnumerable<DataAccessLayer.Entities.User> Participants { get; set; }
    }
}
