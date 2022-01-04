using Core.DataTransferObjects.Project;

namespace BusinessLayer.Commands.Command.Projects
{
    public class UpdateProjectCommand : CommandBase
    {
        public UpdateProjectDTO UpdateProjectDTO { get; private set; }

        public UpdateProjectCommand(UpdateProjectDTO updateProjectDTO)
        {
            this.UpdateProjectDTO = updateProjectDTO;
        }
    }
}
