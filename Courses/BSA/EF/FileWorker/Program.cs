using System;

using FileWorker.Services;

using Microsoft.Extensions.Configuration;

using QueueService.Interfaces;

using Unity;

namespace FileWorker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start file worker");

            IUnityContainer container = ServicesConfiguration.Container;

            // run worker
            using (MessageService messageService = new MessageService(container.Resolve<IConnectionProvider>(), container.Resolve<IConfiguration>()))
            {
                messageService.Run();
            }

            Console.WriteLine("End file worker");
        }

    }
}
