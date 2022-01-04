namespace Core.DataTransferObjects.RabbitMQ
{
    /// <summary>
    /// File worker serialize this fata to file
    /// <para/>
    /// Server read data from file in this format
    /// </summary>
    public class WorkerData
    {
        public ControllerActionName ControllerActionName { get; set; }

        public System.DateTime Date { get; set; }

        public override string ToString()
        {
            return $"[{Date.ToShortDateString()}] {ControllerActionName.ControllerName} - {ControllerActionName.ActionName}";
        }
    }
}
