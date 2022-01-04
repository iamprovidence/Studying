namespace BusinessLayer.Interfaces
{
    public interface ICommandHandler<in TCommand, out TResult> where TCommand : ICommand<TResult>
    {
        TResult Execute(TCommand command);
    }
}
