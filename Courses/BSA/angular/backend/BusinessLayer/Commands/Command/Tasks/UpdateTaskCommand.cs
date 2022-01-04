using Core.DataTransferObjects.Task;

namespace BusinessLayer.Commands.Command.Tasks
{
    public class UpdateTaskCommand : CommandBase
    {
        public UpdateTaskDTO UpdateTaskDTO { get; private set; }

        public UpdateTaskCommand(UpdateTaskDTO updateTaskDTO)
        {
            this.UpdateTaskDTO = updateTaskDTO;
        }
    }
}
