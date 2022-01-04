using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

using DataAccessLayer.Entities;

using BusinessLayer.Interfaces;
using BusinessLayer.Queries.Handler.Teams;
using BusinessLayer.Queries.Query.Teams;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.Commands;
using BusinessLayer.Commands.Handler.Teams;
using BusinessLayer.Commands.Command.Teams;

namespace ProjectStructure.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamsController : ControllerBase
    {
        // FIELDS
        IQueryProcessor queryProcessor;
        ICommandProcessor commandProcessor;

        // CONSTRUCTORS
        public TeamsController(IQueryProcessor queryProcessor, ICommandProcessor commandProcessor)
        {
            this.queryProcessor = queryProcessor;
            this.commandProcessor = commandProcessor;
        }

        // ACTIONS
        // GET api/teams/
        [HttpGet]
        public Task<IEnumerable<Team>> Get()
        {
            return queryProcessor.ProcessAsync<TeamQueryHandler, AllTeamQuery, IEnumerable<Team>>(new AllTeamQuery());
        }

        // GET api/teams/limited/?participantAge=9
        [Route("limited")]
        [HttpGet("participantAge")]
        public Task<IEnumerable<TeamUsersDTO>> GetTeamByAgeLimit(int participantAge)
        {
            return queryProcessor.ProcessAsync<TeamQueryHandler, TeamByAgeLimitQuery, IEnumerable<TeamUsersDTO>>(new TeamByAgeLimitQuery(participantAge));
        }
        
        // GET api/teams/participants_amount/
        [HttpGet]
        [Route("participants_amount")]
        public Task<IDictionary<int, int>> GetByParticipantsAmount()
        {
            return queryProcessor.ProcessAsync<TeamQueryHandler, UserInTeamAmountQuery, IDictionary<int, int>>(new UserInTeamAmountQuery());
        }

        // PATCH api/teams
        [HttpPatch]
        public Task<CommandResponse> Patch([FromBody] Core.DataTransferObjects.Team.RenameTeamDTO value)
        {
            return commandProcessor.ProcessAsync<TeamCommandHandler, RenameTeamCommand, CommandResponse>(new RenameTeamCommand(value));
        }

        // POST: api/tasks
        [HttpPost]
        public Task<CommandResponse> Post([FromBody] Core.DataTransferObjects.Team.CreateTeamDTO value)
        {
            return commandProcessor.ProcessAsync<TeamCommandHandler, CreateTeamCommand, CommandResponse>(new CreateTeamCommand(value, HttpContext.RequestServices));
        }

        // DELETE: api/teams/5
        [HttpDelete("{id}")]
        public Task<CommandResponse> Delete(int id)
        {
            return commandProcessor.ProcessAsync<TeamCommandHandler, DeleteTeamCommand, CommandResponse>(new DeleteTeamCommand(id));
        }

    }
}
