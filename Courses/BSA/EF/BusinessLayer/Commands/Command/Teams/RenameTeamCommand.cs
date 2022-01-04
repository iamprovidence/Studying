namespace BusinessLayer.Commands.Command.Teams
{
    public class RenameTeamCommand : CommandBase
    {
        public Core.DataTransferObjects.Team.RenameTeamDTO RenameTeamDTO { get; private set; }

        public RenameTeamCommand(Core.DataTransferObjects.Team.RenameTeamDTO renameTeamDTO)
        {
            this.RenameTeamDTO = renameTeamDTO;
        }
    }
}
