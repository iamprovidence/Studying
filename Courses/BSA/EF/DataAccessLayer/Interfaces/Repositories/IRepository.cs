using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataAccessLayer.Interfaces.Repositories
{
    public interface IRepository<TEntity> where TEntity : IEntity, new()
    {
        int Count();
        int Count(Expression<Func<TEntity, bool>> predicate);
        int MaxId();

        TEntity Get(int id);
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
                                 Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        bool Insert(TEntity entity);

        bool Delete(int id);
        bool Delete(TEntity entityToDelete);
    }
}