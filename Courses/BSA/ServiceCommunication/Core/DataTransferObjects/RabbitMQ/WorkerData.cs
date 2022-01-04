namespace Core.DataTransferObjects.RabbitMQ
{
    /// <summary>
    /// FileWorker serialize this data to file
    /// <para/>
    /// Server read data from file in this format
    /// <para/>
    /// Bad naming...  \ (-_-) /
    /// </summary>
    public class WorkerData
    {
        public ControllerActionName ControllerActionName { get; set; }

        public System.DateTime Date { get; set; }
    }
}
