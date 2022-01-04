namespace BusinessLayer.Interfaces
{
    public interface IQueryProcessor
    {
        TResponse Process<THandler, TQuery, TResponse>(TQuery query)
               where THandler : IQueryHandler<TQuery, TResponse>, IUnitOfWorkSettable, new()
               where TQuery : IQuery<TResponse>;
    }
}
