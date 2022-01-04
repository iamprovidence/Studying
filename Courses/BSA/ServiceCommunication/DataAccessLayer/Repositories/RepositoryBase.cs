using System;
using System.Linq;
using System.Collections.Generic;

using DataAccessLayer.Interfaces;
using DataAccessLayer.Interfaces.Repositories;

namespace DataAccessLayer.Repositories
{
    public abstract class RepositoryBase<TEntity> : IDataProviderSettable, IRepository<TEntity> where TEntity : Entities.EntityBase, new()                                                    
    {
        // FIELDS
        protected IDataProvider dataProvider;
        protected IList<TEntity> entities;

        // CONSTRUCTORS
        public RepositoryBase()
        {
            dataProvider = null;
            entities = null;
        }
        public abstract void SetDataProvider(IDataProvider dataProvider);

        // METHODS
        public virtual int Count()
        {
            return entities.Count();
        }

        public virtual int Count(Func<TEntity, bool> predicate)
        {
            return entities.Count(predicate);
        }

        public virtual bool Delete(int id)
        {
            TEntity entity = entities.FirstOrDefault(e => e.Id == id);
            return this.Delete(entity);
        }

        public virtual bool Delete(TEntity entityToDelete)
        {
            if (entityToDelete != null)
            {
                return entities.Remove(entityToDelete);
            }
            return false;
        }
        
        public virtual TEntity Get(int id)
        {
            return entities.FirstOrDefault(e => e.Id == id);
        }

        public virtual IEnumerable<TEntity> Get(Func<TEntity, bool> filter = null, 
                                                Func<IEnumerable<TEntity>, IOrderedEnumerable<TEntity>> orderBy = null)
        {
            IEnumerable<TEntity> query = entities;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query;
        }

        public virtual bool Insert(TEntity entity)
        {
            if (entity == null) return false;

            entities.Add(entity);
            return true;
        }

        public int MaxId()
        {
            return entities.Max(t => t.Id);
        }
    }
}
