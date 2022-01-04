using Core.DataTransferObjects.RabbitMQ;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Interfaces
{
    public interface IFileService
    {
        Task<IEnumerable<WorkerData>> ActionCallListAsync();
    }
}
