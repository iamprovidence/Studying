using Unity;

namespace Client
{
    internal static class ServicesConfiguration
    {
        static IUnityContainer container;

        static ServicesConfiguration()
        {
            container = new UnityContainer();
            Configure();
        }
        public static void Configure()
        {
            container.RegisterType<Interfaces.IServiceManager, ServiceManagers.RestApiServiceManager>();            
        }
        public static IUnityContainer Container => container;
    }
}
