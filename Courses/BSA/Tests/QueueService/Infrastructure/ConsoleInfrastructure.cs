using Unity;

using RabbitMQ.Client;

namespace QueueService.Infrastructure
{
    public static class ConsoleInfrastructure
    {
        static IUnityContainer container;

        static ConsoleInfrastructure()
        {
            container = new UnityContainer();
            Configure();
        }
        public static void Configure()
        {
            container.RegisterType<IConnectionFactory, QueueServices.DefaultConnectionFactory>();
            container.RegisterType<Interfaces.IConnectionProvider, QueueServices.ConnectionProvider>();
        }
        public static IUnityContainer Container => container;
    }
}
