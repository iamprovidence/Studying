using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public virtual Task<int> CountAsync()
        {
            return entities.CountAsync();
        }

        public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return entities.CountAsync(predicate);
        }

        public virtual Task<bool> DeleteAsync(int id)
        {
            TEntity entity = entities.FirstOrDefault(e => e.Id == id);
            return this.DeleteAsync(entity);
        }

        public virtual async Task<bool> DeleteAsync(TEntity entityToDelete)
        {
            if (entityToDelete != null)
            {
                await Task.Run(() => entities.Remove(entityToDelete));
                return true;
            }
            return false;
        }
        
        public virtual Task<TEntity> GetAsync(int id)
        {
            return entities.FirstOrDefaultAsync(e => e.Id == id);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null,
                                                           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = entities;

            if (filter != null)
            {
                query = await Task.Run(() => query.Where(filter));
            }

            if (orderBy != null)
            {
                query = await Task.Run(() => orderBy(query));
            }

            return query;
        }

        public virtual async Task<bool> InsertAsync(TEntity entity)
        {
            if (entity == null) return false;

            await entities.AddAsync(entity);
            return true;
        }

        public virtual Task<int> MaxIdAsync()
        {
            return entities.MaxAsync(t => t.Id);
        }
    }
}
