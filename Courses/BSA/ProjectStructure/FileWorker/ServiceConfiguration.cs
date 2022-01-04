using Microsoft.Extensions.Configuration;

using QueueService.Infrastructure;

using Unity;

namespace FileWorker
{
    public static class ServicesConfiguration
    {
        static IUnityContainer container;

        static ServicesConfiguration()
        {
            container = ConsoleInfrastructure.Container;
            Configure();
        }
        public static void Configure()
        {
            container.RegisterFactory<IConfiguration>(factory => new ConfigurationBuilder()
                                                                .SetBasePath(System.AppDomain.CurrentDomain.BaseDirectory)
                                                                .AddJsonFile("appsettings.json")
                                                                .Build());
        }
        public static IUnityContainer Container => container;
    }
}
