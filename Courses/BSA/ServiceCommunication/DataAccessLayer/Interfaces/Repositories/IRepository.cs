using System;
using System.Linq;
using System.Collections.Generic;

namespace DataAccessLayer.Interfaces.Repositories
{
    public interface IRepository<TEntity> where TEntity : IEntity, new()
    {
        int Count();
        int Count(Func<TEntity, bool> predicate);
        int MaxId();

        TEntity Get(int id);
        IEnumerable<TEntity> Get(Func<TEntity, bool> filter = null,
                                 Func<IEnumerable<TEntity>, IOrderedEnumerable<TEntity>> orderBy = null);

        bool Insert(TEntity entity);

        bool Delete(int id);
        bool Delete(TEntity entityToDelete);
    }
}