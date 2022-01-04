namespace BusinessLayer.DataTransferObjects
{
    public class TeamUsersDTO : System.IComparable<TeamUsersDTO>
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }

        public System.Collections.Generic.IEnumerable<DataAccessLayer.Entities.User> Participants { get; set; }

        public int CompareTo(TeamUsersDTO other)
        {
            return this.TeamId.CompareTo(other.TeamId);
        }
    }
}
