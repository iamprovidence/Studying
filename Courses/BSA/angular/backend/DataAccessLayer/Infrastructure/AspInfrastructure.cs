using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccessLayer.Infrastructure
{
    public static class AspInfrastructure
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<Interfaces.IDataProvider, DataProviders.SmallDataProvider>();
            services.AddScoped<Interfaces.IDbInitializer, Initializers.DefaultDbInitializer>();

            services.AddDbContext<DbContext, Context.BinaryDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Scoped);
            
            services.AddScoped(typeof(Interfaces.IUnitOfWork), typeof(Context.UnitOfWork));            
        }
        public static bool IsInternetConnectionEstablished()
        {
            return System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
        }
    }
}
