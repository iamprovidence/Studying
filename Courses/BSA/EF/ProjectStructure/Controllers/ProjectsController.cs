using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

using DataAccessLayer.Entities;

using BusinessLayer.Interfaces;
using BusinessLayer.Queries.Handler.Projects;
using BusinessLayer.Queries.Query.Projects;
using BusinessLayer.Commands;
using BusinessLayer.Commands.Handler.Users;
using BusinessLayer.Commands.Command.Projects;

namespace ProjectStructure.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        // FIELDS
        IQueryProcessor queryProcessor;
        ICommandProcessor commandProcessor;

        // CONSTRUCTORS
        public ProjectsController(IQueryProcessor queryProcessor, ICommandProcessor commandProcessor)
        {
            this.queryProcessor = queryProcessor;
            this.commandProcessor = commandProcessor;
        }

        // ACTIONS
        // GET api/projects
        [HttpGet]
        public IEnumerable<Project> Get()
        {
            return queryProcessor.Process<ProjectQueryHandler, AllProjectQuery, IEnumerable<Project>>(new AllProjectQuery());
        }

        // GET api/projects/{projectId}
        [HttpGet("{projectId}")]
        public Project Get(int projectId)
        {
            return queryProcessor.Process<ProjectQueryHandler, SingleProjectQuery, Project>(new SingleProjectQuery(projectId));
        }
        
        // GET api/projects/task_per_project/?userId=5
        [HttpGet("userId")]
        [Route("task_per_project")]
        public IDictionary<string, int> GetTasksAmountPerProject(int userId)
        {            
            return queryProcessor.Process<ProjectQueryHandler, TasksAmountPerProjectQuery, IDictionary<string, int>>(new TasksAmountPerProjectQuery(userId)); ;
        }

        // GET api/projects/last_project/?userId=5
        [HttpGet("userId")]
        [Route("last_project")]
        public Project GetLastProject(int userId)
        {
            return queryProcessor.Process<ProjectQueryHandler, LastProjectQuery, Project>(new LastProjectQuery(userId)); ;
        }
        
        // POST: api/projects
        [HttpPost]
        public CommandResponse Post([FromBody] Core.DataTransferObjects.Project.CreateProjectDTO value)
        {
            return commandProcessor.Process<ProjectCommandHandler, CreateProjectCommand, CommandResponse>(new CreateProjectCommand(value, HttpContext.RequestServices));
        }

        // DELETE: api/projects/5
        [HttpDelete("{id}")]
        public CommandResponse Delete(int id)
        {
            return commandProcessor.Process<ProjectCommandHandler, DeleteProjectCommand, CommandResponse>(new DeleteProjectCommand(id));
        }
    }
}
