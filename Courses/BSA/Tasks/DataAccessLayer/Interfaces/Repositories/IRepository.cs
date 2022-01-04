using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces.Repositories
{
    public interface IRepository<TEntity> where TEntity : IEntity, new()
    {
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
        Task<int> MaxIdAsync();

        Task<TEntity> GetAsync(int id);
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null,
                                            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        Task<bool> InsertAsync(TEntity entity);

        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteAsync(TEntity entityToDelete);
    }
}