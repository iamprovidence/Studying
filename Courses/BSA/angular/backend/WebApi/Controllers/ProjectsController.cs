using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using DataAccessLayer.Entities;

using BusinessLayer.Interfaces;
using BusinessLayer.Queries.Handler.Projects;
using BusinessLayer.Queries.Query.Projects;
using BusinessLayer.Commands;
using BusinessLayer.Commands.Handler.Users;
using BusinessLayer.Commands.Command.Projects;

using Core.DataTransferObjects.Project;

namespace WebApi.Controllers
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
        public Task<IEnumerable<ProjectListDTO>> Get()
        {
            return queryProcessor.ProcessAsync<ProjectQueryHandler, AllProjectQuery, IEnumerable<ProjectListDTO>>(new AllProjectQuery());
        }

        // GET api/projects/{projectId}
        [HttpGet("{projectId}")]
        public Task<ProjectViewDTO> Get(int projectId)
        {
            return queryProcessor.ProcessAsync<ProjectQueryHandler, SingleProjectQuery, ProjectViewDTO>(new SingleProjectQuery(projectId));
        }
        
        // GET api/projects/task_per_project/?userId=5
        [HttpGet("userId")]
        [Route("task_per_project")]
        public Task<IDictionary<string, int>> GetTasksAmountPerProject(int userId)
        {            
            return queryProcessor.ProcessAsync<ProjectQueryHandler, TasksAmountPerProjectQuery, IDictionary<string, int>>(new TasksAmountPerProjectQuery(userId)); ;
        }

        // GET api/projects/last_project/?userId=5
        [HttpGet("userId")]
        [Route("last_project")]
        public Task<Project> GetLastProject(int userId)
        {
            return queryProcessor.ProcessAsync<ProjectQueryHandler, LastProjectQuery, Project>(new LastProjectQuery(userId)); ;
        }

        // PATCH: api/projects
        [HttpPatch]
        public Task<CommandResponse> Patch([FromBody] UpdateProjectDTO value)
        {
            if (value == null) throw new System.ArgumentNullException(nameof(value));

            return commandProcessor.ProcessAsync<ProjectCommandHandler, UpdateProjectCommand, CommandResponse>(new UpdateProjectCommand(value));
        }

        // POST: api/projects
        [HttpPost]
        public Task<CommandResponse> Post([FromBody] CreateProjectDTO value)
        {
            if (value == null) throw new System.ArgumentNullException(nameof(value));

            return commandProcessor.ProcessAsync<ProjectCommandHandler, CreateProjectCommand, CommandResponse>(new CreateProjectCommand(value, HttpContext.RequestServices));
        }

        // DELETE: api/projects/5
        [HttpDelete("{id}")]
        public Task<CommandResponse> Delete(int id)
        {
            return commandProcessor.ProcessAsync<ProjectCommandHandler, DeleteProjectCommand, CommandResponse>(new DeleteProjectCommand(id));
        }
        
    }
}
