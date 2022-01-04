using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

using Newtonsoft.Json;

using QueueService.Interfaces;

using SerializationResult = Core.DataTransferObjects.RabbitMQ.SerializationResult;

namespace ProjectStructure.HostedServices
{
    /// <summary>
    /// Asynchronous checks if FileWorker had enqueue any data
    /// </summary>
    public class MessageService : BackgroundService, System.IDisposable
    {
        // FIELDS
        IConsumer consumer;
        IHubContext<Hubs.MessageHub> hubContext;

        // CONSTRUCTORS
        public MessageService(IConnectionProvider factory, IHubContext<Hubs.MessageHub> hubContext, IConfiguration configuration)
        {
            QueueService.Models.Settings settings = new QueueService.Models.Settings();
            configuration.Bind("RabbitMq:FromWorkerToServer", settings);

            this.consumer = factory.Connect(settings);

            this.hubContext = hubContext;
        }

        public override void Dispose()
        {
            base.Dispose();
            consumer.Dispose();
        }
        
        // METHODS
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // if FileWorker send serialization results... 
                QueueService.Models.ReceiveData receiveData = await Task.Run(() => consumer.Receive(2500));

                if (receiveData != null)
                {
                    // ..resends it to all clients with SignalR
                    SerializationResult serializationResult = JsonConvert.DeserializeObject<SerializationResult>(receiveData.Message);
                    await hubContext.Clients.All.SendAsync("NewMessage", serializationResult);

                    // .. processed it
                    consumer.SetAcknowledge(receiveData.DeliveryTag, processed: true);
                }
            }
        }

    }
}
