using DataAccessLayer.Interfaces;
using DataAccessLayer.Interfaces.Repositories;

using System.Collections.Generic;

namespace DataAccessLayer.Context
{
    public class UnitOfWork : IUnitOfWork
    {
        // FIELDS
        private readonly IDictionary<System.Type, object> repositoriesFactory;
        private readonly IDataProvider dataProvider;

        // CONSTRUCTORS
        public UnitOfWork(IDataProvider dataProvider)
        {
            this.repositoriesFactory = new Dictionary<System.Type, object>();
            this.dataProvider = dataProvider;
        }

        // METHODS
        public TRepository GetRepository<TEntity, TRepository>()
            where TEntity : IEntity, new()
            where TRepository : IRepository<TEntity>, IDataProviderSettable, new()
        {
            System.Type key = typeof(TEntity);

            // add lazy loaded repository
            if (!repositoriesFactory.ContainsKey(key))
            {
                TRepository repository = new TRepository();
                repository.SetDataProvider(dataProvider);
                repositoriesFactory.Add(key, repository);
            }

            // return repository
            return (TRepository)repositoriesFactory[key];
        }

        public int Save()
        {
            throw new System.NotImplementedException("Database is not used. Saving is not allowed.");
        }

        public void Update<TEntity>(TEntity entityToUpdate) where TEntity : class
        {
            throw new System.NotImplementedException("Database is not used. Updating is not allowed.");
        }
    }
}
