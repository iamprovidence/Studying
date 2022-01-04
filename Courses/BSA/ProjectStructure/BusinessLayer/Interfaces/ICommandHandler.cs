namespace BusinessLayer.Interfaces
{
    public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand<TResult>
    {
        System.Threading.Tasks.Task<TResult> ExecuteAsync(TCommand command);
    }
}
