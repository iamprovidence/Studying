namespace DataAccessLayer.Interfaces
{
    public interface IUnitOfWork
    {
        TRepository GetRepository<TEntity, TRepository>()
            where TEntity : IEntity, new()
            where TRepository : Repositories.IRepository<TEntity>, IDataProviderSettable, new();

        void Update<TEntity>(TEntity entityToUpdate) where TEntity : class;
        int Save();
    }
}
