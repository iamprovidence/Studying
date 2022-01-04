using Core.DataTransferObjects.User;

namespace BusinessLayer.Commands.Command.Users
{
    public class CreateUserCommand : CommandBase
    {
        public CreateUserDTO CreateUserDTO { get; private set; }
        public System.IServiceProvider ServiceProvider { get; private set; }

        public CreateUserCommand(CreateUserDTO createUserDTO, System.IServiceProvider serviceProvider)
        {
            this.CreateUserDTO = createUserDTO;
            this.ServiceProvider = serviceProvider;
        }
    }
}
