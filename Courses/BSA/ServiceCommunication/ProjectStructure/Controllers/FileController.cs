using System.Collections.Generic;

using BusinessLayer.Interfaces;
using BusinessLayer.Queries.Handler.File;
using BusinessLayer.Queries.Query.File;

using Core.DataTransferObjects.RabbitMQ;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ProjectStructure.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        // FIELDS
        IQueryProcessor queryProcessor;
        IConfiguration configuration;

        // CONSTRUCTORS
        public FileController(IQueryProcessor queryProcessor, IConfiguration configuration)
        {
            this.queryProcessor = queryProcessor;
            this.configuration = configuration;
        }

        // GET: api/File
        [HttpGet]
        public IEnumerable<WorkerData> Get()
        {
            try
            {                
                return queryProcessor.Process<FileQueryHandler, FileContentQuery, IEnumerable<WorkerData>>(new FileContentQuery(configuration["WorkerSerializeFile"]));
            }
            catch (System.Exception)
            {
                return System.Linq.Enumerable.Empty<WorkerData>();
            }
        }
    }
}
