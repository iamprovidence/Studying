using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using RabbitMQ.Client;

namespace QueueService.Infrastructure
{
    public static class AspInfrastructure
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionFactory, QueueServices.DefaultConnectionFactory>();
            services.AddSingleton<Interfaces.IConnectionProvider, QueueServices.ConnectionProvider>();
        }
    }
}
