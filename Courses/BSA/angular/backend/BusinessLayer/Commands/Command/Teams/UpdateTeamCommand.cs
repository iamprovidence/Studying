namespace BusinessLayer.Commands.Command.Teams
{
    public class UpdateTeamCommand : CommandBase
    {
        public Core.DataTransferObjects.Team.UpdateTeamDTO UpdateTeamDTO { get; private set; }

        public UpdateTeamCommand(Core.DataTransferObjects.Team.UpdateTeamDTO updateTeamDTO)
        {
            this.UpdateTeamDTO = updateTeamDTO;
        }
    }
}
