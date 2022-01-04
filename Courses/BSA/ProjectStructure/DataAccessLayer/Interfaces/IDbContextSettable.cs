namespace DataAccessLayer.Interfaces
{
    public interface IDbContextSettable
    {
        void SetDbContext(Microsoft.EntityFrameworkCore.DbContext dbContext);
    }
}
