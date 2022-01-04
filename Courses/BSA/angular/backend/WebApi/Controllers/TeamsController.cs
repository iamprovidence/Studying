using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

using BusinessLayer.Interfaces;
using BusinessLayer.Queries.Handler.Teams;
using BusinessLayer.Queries.Query.Teams;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.Commands;
using BusinessLayer.Commands.Handler.Teams;
using BusinessLayer.Commands.Command.Teams;

using Core.DataTransferObjects.Team;

namespace WebApi.Controllers
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
        public Task<IEnumerable<TeamListDTO>> Get()
        {
            return queryProcessor.ProcessAsync<TeamQueryHandler, AllTeamQuery, IEnumerable<TeamListDTO>>(new AllTeamQuery());
        }


        // GET api/teams/{teamId}
        [HttpGet("{teamId}")]
        public Task<TeamViewDTO> Get(int teamId)
        {
            return queryProcessor.ProcessAsync<TeamQueryHandler, SingleTeamQuery, TeamViewDTO>(new SingleTeamQuery(teamId));
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
        public Task<CommandResponse> Patch([FromBody] UpdateTeamDTO value)
        {
            if (value == null) throw new System.ArgumentNullException(nameof(value));
            
            return commandProcessor.ProcessAsync<TeamCommandHandler, UpdateTeamCommand, CommandResponse>(new UpdateTeamCommand(value));
        }

        // POST: api/teams
        [HttpPost]
        public Task<CommandResponse> Post([FromBody] CreateTeamDTO value)
        {
            if (value == null) throw new System.ArgumentNullException(nameof(value));

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
