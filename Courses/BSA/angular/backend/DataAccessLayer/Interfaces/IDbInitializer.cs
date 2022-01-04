namespace DataAccessLayer.Interfaces
{
    public interface IDbInitializer
    {
        void Seed(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder);
    }
}
