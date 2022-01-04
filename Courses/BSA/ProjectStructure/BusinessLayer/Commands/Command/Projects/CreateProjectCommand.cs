using Core.DataTransferObjects.Project;

namespace BusinessLayer.Commands.Command.Projects
{
    public class CreateProjectCommand : CommandBase
    {
        public CreateProjectDTO CreateProjectDTO { get; private set; }
        public System.IServiceProvider ServiceProvider { get; private set; }

        public CreateProjectCommand(CreateProjectDTO createProjectDTO, System.IServiceProvider serviceProvider)
        {
            this.CreateProjectDTO = createProjectDTO;
            this.ServiceProvider = serviceProvider;
        }
    }
}
