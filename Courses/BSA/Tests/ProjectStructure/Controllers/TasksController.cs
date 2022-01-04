using BusinessLayer.Interfaces;
using BusinessLayer.Queries.Query.Tasks;
using BusinessLayer.Queries.Handler.Tasks;

using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using BusinessLayer.Commands;
using BusinessLayer.Commands.Command.Tasks;
using BusinessLayer.Commands.Handler.Tasks;

using System.Threading.Tasks;

using TaskDb = DataAccessLayer.Entities.Task;

namespace ProjectStructure.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        // FIELDS
        IQueryProcessor queryProcessor;
        ICommandProcessor commandProcessor;

        // CONSTRUCTORS
        public TasksController(IQueryProcessor queryProcessor, ICommandProcessor commandProcessor)
        {
            this.queryProcessor = queryProcessor;
            this.commandProcessor = commandProcessor;
        }

        // ACTIONS
        // GET api/tasks/
        [HttpGet]
        public Task<IEnumerable<TaskDb>> Get()
        {
            return queryProcessor.ProcessAsync<TaskQueryHandler, AllTaskQuery, IEnumerable<TaskDb>>(new AllTaskQuery());
        }

        // GET api/tasks/{taskId}
        [HttpGet("{taskId}")]
        public Task<TaskDb> Get(int taskId)
        {
            return queryProcessor.ProcessAsync<TaskQueryHandler, SingleTaskQuery, TaskDb>(new SingleTaskQuery(taskId));
        }
        
        // GET api/tasks/short_name/?userId=5
        [HttpGet("userId")]
        [Route("short_name")]
        public Task<IEnumerable<TaskDb>> GetTasksWithShortName(int userId)
        {
            return queryProcessor.ProcessAsync<TaskQueryHandler, ShortTaskQuery, IEnumerable<TaskDb>>(new ShortTaskQuery(userId));
        }

        // GET api/tasks/finished/?userId=5
        [Route("finished")]
        [HttpGet("userId")]
        public Task<IEnumerable<TaskDb>> GetFinished(int userId)
        {
            return queryProcessor.ProcessAsync<TaskQueryHandler, FinishedInYearQuery, IEnumerable<TaskDb>>(new FinishedInYearQuery(userId));
        }

        // GET api/tasks/longest_description/?projectId=5
        [HttpGet("projectId")]
        [Route("longest_description")]
        public Task<TaskDb> GetWithLongestDescription(int projectId)
        {
            return queryProcessor.ProcessAsync<TaskQueryHandler, TaskWithLongestDescriptionQuery, TaskDb>(new TaskWithLongestDescriptionQuery(projectId));
        }
        // GET api/tasks/shortest_name/?projectId=5
        [HttpGet("projectId")]
        [Route("shortest_name")]
        public Task<TaskDb> GetWithShortestName(int projectId)
        {
            return queryProcessor.ProcessAsync<TaskQueryHandler, TaskWithShortestNameQuery, TaskDb>(new TaskWithShortestNameQuery(projectId));
        }

        // GET api/tasks/longest_period/?userId=5
        [HttpGet("userId")]
        [Route("longest_period")]
        public Task<TaskDb> GetLongestTask(int userId)
        {
            return queryProcessor.ProcessAsync<TaskQueryHandler, LongestTaskQuery, TaskDb>(new LongestTaskQuery(userId));
        }

        // GET api/tasks/count/?userId=5&projectId=5
        [HttpGet("userId, projectId")]
        [Route("count")]
        public Task<int> CountTaskPerProject(int userId, int projectId)
        {
            return queryProcessor.ProcessAsync<TaskQueryHandler, CountTaskPerProjectQuery, int>(new CountTaskPerProjectQuery(userId, projectId));
        }

        // GET api/tasks/unfinished/?userId=5
        [HttpGet("userId")]
        [Route("unfinished")]
        public Task<int> CountUnfinishedTask(int userId)
        {
            return queryProcessor.ProcessAsync<TaskQueryHandler, CountUnfinishedTaskQuery, int>(new CountUnfinishedTaskQuery(userId));
        }

        // POST: api/tasks
        [HttpPost]
        public Task<CommandResponse> Post([FromBody] Core.DataTransferObjects.Task.CreateTaskDTO value)
        {
            return commandProcessor.ProcessAsync<TaskCommandHandler, CreateTaskCommand, CommandResponse>(new CreateTaskCommand(value, HttpContext.RequestServices));
        }
        
        // DELETE: api/tasks/5
        [HttpDelete("{id}")]
        public Task<CommandResponse> Delete(int id)
        {
            return commandProcessor.ProcessAsync<TaskCommandHandler, DeleteTaskCommand, CommandResponse>(new DeleteTaskCommand(id));
        }
    }
}