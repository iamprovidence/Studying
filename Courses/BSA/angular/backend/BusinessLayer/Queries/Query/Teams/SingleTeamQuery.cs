namespace BusinessLayer.Queries.Query.Teams
{
    public class SingleTeamQuery : Interfaces.IQuery<Core.DataTransferObjects.Team.TeamViewDTO>
    {
        public int TeamId { get; private set; }

        public SingleTeamQuery(int teamId)
        {
            this.TeamId = teamId;
        }
    }
}
