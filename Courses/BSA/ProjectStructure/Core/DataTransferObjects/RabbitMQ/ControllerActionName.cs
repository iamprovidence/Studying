namespace Core.DataTransferObjects.RabbitMQ
{
    /// <summary>
    /// Server enqueues this data for FileWorker
    /// </summary>
    public class ControllerActionName
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
    }
}
