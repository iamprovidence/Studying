using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

using DataAccessLayer.Interfaces;
using DataAccessLayer.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public abstract class RepositoryBase<TEntity> : IDbContextSettable, IRepository<TEntity> where TEntity : Entities.EntityBase, new()
    {
        // FIELDS
        protected DbContext dbContext;
        protected DbSet<TEntity> entities;

        // CONSTRUCTORS
        public RepositoryBase()
        {
            dbContext = null;
            entities = null;
        }
        public void SetDbContext(DbContext dbContext)
        {
            if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));

            this.dbContext = dbContext;
            this.entities = dbContext.Set<TEntity>();
        }

        // METHODS
        public virtual int Count()
        {
            return entities.Count();
        }

        public virtual int Count(Expression<Func<TEntity, bool>> predicate)
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
                entities.Remove(entityToDelete);
                return true;
            }
            return false;
        }
        
        public virtual TEntity Get(int id)
        {
            return entities.FirstOrDefault(e => e.Id == id);
        }

        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
                                                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = entities;

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
