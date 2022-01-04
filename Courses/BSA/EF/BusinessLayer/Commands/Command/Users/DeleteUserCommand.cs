namespace BusinessLayer.Commands.Command.Users
{
    public class DeleteUserCommand : CommandBase
    {
        public int UserId { get; private set; }

        public DeleteUserCommand(int userId)
        {
            this.UserId = userId;
        }
    }
}
