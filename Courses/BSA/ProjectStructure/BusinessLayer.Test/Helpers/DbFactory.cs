using DataAccessLayer.Context;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLayer.Test.Helpers
{
    public static class DbFactory
    {
        public static DbContextOptions<BinaryDbContext> CreateNewContextOptions()
        {
            // Create a fresh service provider, and therefore a fresh 
            // InMemory database instance.
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            return new DbContextOptionsBuilder<BinaryDbContext>()
                .UseInMemoryDatabase(System.Guid.NewGuid().ToString())
                .UseInternalServiceProvider(serviceProvider)
                .Options;
        }
        public static DataAccessLayer.Interfaces.IUnitOfWork CreateDefaultUnitOfWork(DataAccessLayer.Interfaces.IDbInitializer dbInitializer)
        {
            return new UnitOfWork(new BinaryDbContext(CreateNewContextOptions(), dbInitializer));
        }
    }
}
