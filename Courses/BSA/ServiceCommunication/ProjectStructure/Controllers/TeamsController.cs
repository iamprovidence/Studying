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
        public IEnumerable<Team> Get()
        {
            return queryProcessor.Process<TeamQueryHandler, AllTeamQuery, IEnumerable<Team>>(new AllTeamQuery());
        }

        // GET api/teams/limited/?participantAge=9
        [Route("limited")]
        [HttpGet("participantAge")]
        public IEnumerable<TeamUsersDTO> GetTeamByAgeLimit(int participantAge)
        {
            return queryProcessor.Process<TeamQueryHandler, TeamByAgeLimitQuery, IEnumerable<TeamUsersDTO>>(new TeamByAgeLimitQuery(participantAge));
        }
        
        // GET api/teams/participants_amount/
        [HttpGet]
        [Route("participants_amount")]
        public IDictionary<int, int> GetByParticipantsAmount()
        {
            return queryProcessor.Process<TeamQueryHandler, UserInTeamAmountQuery, IDictionary<int, int>>(new UserInTeamAmountQuery());
        }

        // PATCH api/teams
        [HttpPatch]
        public CommandResponse Patch([FromBody] Core.DataTransferObjects.Team.RenameTeamDTO value)
        {
            return commandProcessor.Process<TeamCommandHandler, RenameTeamCommand, CommandResponse>(new RenameTeamCommand(value));
        }

        // POST: api/tasks
        [HttpPost]
        public CommandResponse Post([FromBody] Core.DataTransferObjects.Team.CreateTeamDTO value)
        {
            return commandProcessor.Process<TeamCommandHandler, CreateTeamCommand, CommandResponse>(new CreateTeamCommand(value, HttpContext.RequestServices));
        }

        // DELETE: api/teams/5
        [HttpDelete("{id}")]
        public CommandResponse Delete(int id)
        {
            return commandProcessor.Process<TeamCommandHandler, DeleteTeamCommand, CommandResponse>(new DeleteTeamCommand(id));
        }

    }
}
