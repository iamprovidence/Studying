using Core.DataTransferObjects.Team;

namespace BusinessLayer.Commands.Command.Teams
{
    public class CreateTeamCommand : CommandBase
    {
        public CreateTeamDTO CreateTeamDTO { get; private set; }
        public System.IServiceProvider ServiceProvider { get; private set; }

        public CreateTeamCommand(CreateTeamDTO createTeamDTO, System.IServiceProvider serviceProvider)
        {
            this.CreateTeamDTO = createTeamDTO;
            this.ServiceProvider = serviceProvider;
        }
    }
}
