using DataAccessLayer.Entities;

using BusinessLayer.Interfaces;
using BusinessLayer.Queries.Query.Tasks;
using BusinessLayer.Queries.Handler.Tasks;

using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using BusinessLayer.Commands;
using BusinessLayer.Commands.Command.Tasks;
using BusinessLayer.Commands.Handler.Tasks;

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
        public IEnumerable<Task> Get()
        {
            return queryProcessor.Process<TaskQueryHandler, AllTaskQuery, IEnumerable<Task>>(new AllTaskQuery());
        }

        // GET api/tasks/{taskId}
        [HttpGet("{taskId}")]
        public Task Get(int taskId)
        {
            return queryProcessor.Process<TaskQueryHandler, SingleTaskQuery, Task>(new SingleTaskQuery(taskId));
        }
        
        // GET api/tasks/short_name/?userId=5
        [HttpGet("userId")]
        [Route("short_name")]
        public IEnumerable<Task> GetTasksWithShortName(int userId)
        {
            return queryProcessor.Process<TaskQueryHandler, ShortTaskQuery, IEnumerable<Task>>(new ShortTaskQuery(userId));
        }

        // GET api/tasks/finished/?userId=5
        [Route("finished")]
        [HttpGet("userId")]
        public IEnumerable<Task> GetFinished(int userId)
        {
            return queryProcessor.Process<TaskQueryHandler, FinishedInYearQuery, IEnumerable<Task>>(new FinishedInYearQuery(userId));
        }

        // GET api/tasks/longest_description/?projectId=5
        [HttpGet("projectId")]
        [Route("longest_description")]
        public Task GetWithLongestDescription(int projectId)
        {
            return queryProcessor.Process<TaskQueryHandler, TaskWithLongestDescriptionQuery, Task>(new TaskWithLongestDescriptionQuery(projectId));
        }
        // GET api/tasks/shortest_name/?projectId=5
        [HttpGet("projectId")]
        [Route("shortest_name")]
        public Task GetWithShortestName(int projectId)
        {
            return queryProcessor.Process<TaskQueryHandler, TaskWithShortestNameQuery, Task>(new TaskWithShortestNameQuery(projectId));
        }

        // GET api/tasks/longest_period/?userId=5
        [HttpGet("userId")]
        [Route("longest_period")]
        public Task GetLongestTask(int userId)
        {
            return queryProcessor.Process<TaskQueryHandler, LongestTaskQuery, Task>(new LongestTaskQuery(userId));
        }

        // GET api/tasks/count/?userId=5&projectId=5
        [HttpGet("userId, projectId")]
        [Route("count")]
        public int CountTaskPerProject(int userId, int projectId)
        {
            return queryProcessor.Process<TaskQueryHandler, CountTaskPerProjectQuery, int>(new CountTaskPerProjectQuery(userId, projectId));
        }

        // GET api/tasks/unfinished/?userId=5
        [HttpGet("userId")]
        [Route("unfinished")]
        public int CountUnfinishedTask(int userId)
        {
            return queryProcessor.Process<TaskQueryHandler, CountUnfinishedTaskQuery, int>(new CountUnfinishedTaskQuery(userId));
        }

        // POST: api/tasks
        [HttpPost]
        public CommandResponse Post([FromBody] Core.DataTransferObjects.Task.CreateTaskDTO value)
        {
            return commandProcessor.Process<TaskCommandHandler, CreateTaskCommand, CommandResponse>(new CreateTaskCommand(value, HttpContext.RequestServices));
        }
        
        // DELETE: api/tasks/5
        [HttpDelete("{id}")]
        public CommandResponse Delete(int id)
        {
            return commandProcessor.Process<TaskCommandHandler, DeleteTaskCommand, CommandResponse>(new DeleteTaskCommand(id));
        }
    }
}