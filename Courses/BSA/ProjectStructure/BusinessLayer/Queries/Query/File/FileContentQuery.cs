using Core.DataTransferObjects.RabbitMQ;

using System.Collections.Generic;

namespace BusinessLayer.Queries.Query.File
{
    public class FileContentQuery : Interfaces.IQuery<IEnumerable<WorkerData>>
    {
        public string FilePath { get; private set; }
        public Enums.SortingOrder SortingOrder { get; set; } = Enums.SortingOrder.Desc;

        public FileContentQuery(string filePath)
        {
            this.FilePath = filePath;
        }
    }
}
