using Core.DataTransferObjects.User;

namespace BusinessLayer.Commands.Command.Users
{
    public class CreateUserCommand : CommandBase
    {
        public CreateUserDTO CreateUserDTO { get; private set; }

        public CreateUserCommand(CreateUserDTO createUserDTO)
        {
            this.CreateUserDTO = createUserDTO;
        }
    }
}
