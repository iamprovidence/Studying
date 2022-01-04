using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IQueryProcessor
    {
        Task<TResponse> ProcessAsync<THandler, TQuery, TResponse>(TQuery query)
               where THandler : IQueryHandler<TQuery, TResponse>, IUnitOfWorkSettable, IMapperSettable, new()
               where TQuery : IQuery<TResponse>;
    }
}
