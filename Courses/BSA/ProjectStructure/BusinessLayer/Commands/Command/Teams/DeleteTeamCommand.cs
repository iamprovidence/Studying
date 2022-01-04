namespace BusinessLayer.Commands.Command.Teams
{
    public class DeleteTeamCommand : CommandBase
    {
        public int TeamId { get; private set; }

        public DeleteTeamCommand(int teamId)
        {
            this.TeamId = teamId;
        }
    }
}
