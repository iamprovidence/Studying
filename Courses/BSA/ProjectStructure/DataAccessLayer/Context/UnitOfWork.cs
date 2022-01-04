using DataAccessLayer.Interfaces;
using DataAccessLayer.Interfaces.Repositories;

using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Context
{
    public class UnitOfWork : IUnitOfWork
    {
        // FIELDS
        private readonly IDictionary<System.Type, object> repositoriesFactory;
        private readonly DbContext dbContext;

        // CONSTRUCTORS
        public UnitOfWork(DbContext dbContext)
        {
            this.repositoriesFactory = new Dictionary<System.Type, object>();
            this.dbContext = dbContext;
        }

        public void Dispose()
        {
            this.dbContext.Dispose();
        }

        // METHODS
        public TRepository GetRepository<TEntity, TRepository>()
            where TEntity : IEntity, new()
            where TRepository : IRepository<TEntity>, IDbContextSettable, new()
        {
            System.Type key = typeof(TEntity);

            // add lazy loaded repository
            if (!repositoriesFactory.ContainsKey(key))
            {
                TRepository repository = new TRepository();
                repository.SetDbContext(dbContext);
                repositoriesFactory.Add(key, repository);
            }

            // return repository
            return (TRepository)repositoriesFactory[key];
        }

        public Task<int> SaveAsync()
        {
            return dbContext.SaveChangesAsync();
        }

        public void Update<TEntity>(TEntity entityToUpdate) where TEntity : class
        {
            if (dbContext.Entry(entityToUpdate).State == EntityState.Detached)
            {
                dbContext.Set<TEntity>().Attach(entityToUpdate);
            }
            dbContext.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}
