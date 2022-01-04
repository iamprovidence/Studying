using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

using Core.DataTransferObjects.RabbitMQ;

using Newtonsoft.Json;

using QueueService.Interfaces;

namespace ProjectStructure.Filters
{
    /// <summary>
    /// Filter, that before each methods enqueues Controller and Action's names for File Worker
    /// </summary>
    public class ExecutedActionFilterAttribute : Attribute, IAsyncActionFilter, IDisposable
    {
        // FIELDS
        IProducer producer;

        // CONSTRUCTORS
        public ExecutedActionFilterAttribute(IConnectionProvider factory, IConfiguration configuration)
        {
            QueueService.Models.Settings settings = new QueueService.Models.Settings();
            configuration.Bind("RabbitMq:FromServerToWorker", settings);

            this.producer = factory.Open(settings);
        }

        public void Dispose()
        {
            producer.Dispose();
        }

        // METHODS
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // gather data
            ControllerActionName data = new ControllerActionName
            {
                ControllerName = (string)context.RouteData.Values["Controller"],
                ActionName = (string)context.RouteData.Values["Action"]
            };

            // serialize object
            string objectJson = JsonConvert.SerializeObject(data);
            
            // send object to FileWorker
            producer.Send(objectJson);            

            await next();
        }
    }
}
