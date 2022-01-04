namespace BusinessLayer.Interfaces
{
    public interface ICommandProcessor
    {
        TResponse Process<THandler, TCommand, TResponse>(TCommand command)
               where THandler : ICommandHandler<TCommand, TResponse>, IUnitOfWorkSettable, new()
               where TCommand : ICommand<TResponse>;
    }
}
