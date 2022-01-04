using System.Collections.Generic;
using System.Threading.Tasks;

using BusinessLayer.Interfaces;
using BusinessLayer.Queries.Handler.File;
using BusinessLayer.Queries.Query.File;

using Core.DataTransferObjects.RabbitMQ;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace WebApi.Controllers
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
        public Task<IEnumerable<WorkerData>> Get()
        {
            try
            {                
                return queryProcessor.ProcessAsync<FileQueryHandler, FileContentQuery, IEnumerable<WorkerData>>(new FileContentQuery(configuration["WorkerSerializeFile"]));
            }
            catch (System.Exception)
            {
                return null;
            }
        }
    }
}
