using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IQueryHandler<in TQuery, TResponse> where TQuery : IQuery<TResponse>
    {
        Task<TResponse> HandleAsync(TQuery query);
    }
}
