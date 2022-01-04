using System;
using System.IO;

using QueueService.Models;
using QueueService.Interfaces;

using Core.DataTransferObjects.RabbitMQ;

using Newtonsoft.Json;

using Microsoft.Extensions.Configuration;

namespace FileWorker.Services
{
    internal class MessageService : IDisposable
    {
        // FIELDS
        IConfiguration configuration;

        IConsumer consumer;
        IProducer producer;

        // CONSTRUCTORS
        public MessageService(IConnectionProvider connectionProvider, IConfiguration configuration)
        {
            this.configuration = configuration;

            Settings settings = new Settings();

            // create producer
            configuration.Bind("RabbitMq:FromWorkerToServer", settings);
            producer = connectionProvider.Open(settings);

            // open consumer
            configuration.Bind("RabbitMq:FromServerToWorker", settings);
            consumer = connectionProvider.Connect(settings);
        }

        public void Dispose()
        {
            consumer.Dispose();
            producer.Dispose();
        }

        // METHODS
        public void Run()
        {
            while (true)
            {
                ReceiveData data = consumer.Receive(int.Parse(configuration["RabbitMq:MillisecondsReceiveTimeout"]));

                if (data != null)
                {
                    // show data
                    Console.WriteLine(data.Message);

                    // try to serialize
                    string errorMessage = string.Empty;
                    try
                    {
                        SerializeData(data.Message);
                    }
                    catch (Exception e)
                    {
                        errorMessage = e.Message;
                    }
                    // send to server serialization status
                    SendSerializationResultData(errorMessage);

                    // processed message
                    consumer.SetAcknowledge(data.DeliveryTag, processed: true);
                }
            }
        }

        // serialize
        private void SerializeData(string messageJson)
        {
            // gather data
            WorkerData workerData = new WorkerData
            {
                ControllerActionName = JsonConvert.DeserializeObject<ControllerActionName>(messageJson),
                Date = DateTime.Now // add date
            };

            // serialize
            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter sw = new StreamWriter(configuration["WorkerSerializeFile"], append: true))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, workerData);
                sw.WriteLine(); // each object from new line, easier to deserialize
            }
        }

        // sending to server
        private void SendSerializationResultData(string errorMessage)
        {
            SerializationResult result = CreateResult(errorMessage);

            string objectJson = JsonConvert.SerializeObject(result);

            producer.Send(objectJson);
        }

        private SerializationResult CreateResult(string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                return new SerializationResult
                {
                    IsSuccessed = true,
                    Message = "Successfully serialized",
                };
            }
            else
            {
                return new SerializationResult
                {
                    IsSuccessed = false,
                    Message = errorMessage
                };
            }
        }
    }
}
