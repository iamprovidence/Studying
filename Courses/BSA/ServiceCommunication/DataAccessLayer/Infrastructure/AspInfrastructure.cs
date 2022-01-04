using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccessLayer.Infrastructure
{
    public static class AspInfrastructure
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // add services
            // singleton - because we want to fetch data only once
            // and save it in memory
            // if change this to "Scoped" modifying would not work
            services.AddSingleton<Interfaces.IDataProvider>(f =>
            {
                if (IsInternetConnectionEstablished()) return new DataProviders.WebApiDataProvider();
                else                                   return new DataProviders.InMemoryDataProvider();
            });
            services.AddSingleton(typeof(Interfaces.IUnitOfWork), typeof(Context.UnitOfWork));
        }
        public static bool IsInternetConnectionEstablished()
        {
            return System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
        }
    }
}
