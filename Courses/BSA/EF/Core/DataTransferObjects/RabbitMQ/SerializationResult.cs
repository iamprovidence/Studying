namespace Core.DataTransferObjects.RabbitMQ
{
    /// <summary>
    /// FileWorker enqueues this data for Server
    /// </summary>
    public class SerializationResult
    {
        public bool IsSuccessed { get; set; }
        public string Message { get; set; }
    }
}
