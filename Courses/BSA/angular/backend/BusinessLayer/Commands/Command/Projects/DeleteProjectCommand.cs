namespace BusinessLayer.Commands.Command.Projects
{
    public class DeleteProjectCommand : CommandBase
    {
        public int ProjectId { get; private set; }

        public DeleteProjectCommand(int projectId)
        {
            this.ProjectId = projectId;
        }
    }
}
