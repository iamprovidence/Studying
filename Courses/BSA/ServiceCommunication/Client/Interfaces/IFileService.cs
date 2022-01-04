namespace Client.Interfaces
{
    public interface IFileService
    {
        System.Collections.Generic.IEnumerable<Core.DataTransferObjects.RabbitMQ.WorkerData> ActionCallList();
    }
}
