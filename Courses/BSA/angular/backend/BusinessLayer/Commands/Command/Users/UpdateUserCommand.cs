using Core.DataTransferObjects.User;

namespace BusinessLayer.Commands.Command.Users
{
    public class UpdateUserCommand : CommandBase
    {
        public UpdateUserDTO UpdateUserDTO { get; private set; }

        public UpdateUserCommand(UpdateUserDTO updateUserDTO)
        {
            this.UpdateUserDTO = updateUserDTO;
        }
    }
}
