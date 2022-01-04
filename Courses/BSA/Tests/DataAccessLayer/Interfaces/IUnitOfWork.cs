namespace DataAccessLayer.Interfaces
{
    public interface IUnitOfWork : System.IDisposable
    {
        TRepository GetRepository<TEntity, TRepository>()
            where TEntity : IEntity, new()
            where TRepository : Repositories.IRepository<TEntity>, IDbContextSettable, new();

        void Update<TEntity>(TEntity entityToUpdate) where TEntity : class;
        System.Threading.Tasks.Task<int> SaveAsync();
    }
}
