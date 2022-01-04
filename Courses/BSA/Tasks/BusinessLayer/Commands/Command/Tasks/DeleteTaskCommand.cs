namespace BusinessLayer.Commands.Command.Tasks
{
    public class DeleteTaskCommand : CommandBase
    {
        public int TaskId { get; private set; }

        public DeleteTaskCommand(int taskId)
        {
            this.TaskId = taskId;
        }
    }
}
