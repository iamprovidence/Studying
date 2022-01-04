using Core.DataTransferObjects.Task;

namespace BusinessLayer.Commands.Command.Tasks
{
    public class CreateTaskCommand : CommandBase
    {
        public CreateTaskDTO CreateTaskDTO { get; private set; }
        public System.IServiceProvider ServiceProvider { get; private set; }

        public CreateTaskCommand(CreateTaskDTO createTaskDTO, System.IServiceProvider serviceProvider)
        {
            this.CreateTaskDTO = createTaskDTO;
            this.ServiceProvider = serviceProvider;
        }
    }
}
