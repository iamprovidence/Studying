using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

using DataAccessLayer.Entities;

using BusinessLayer.Interfaces;
using BusinessLayer.Queries.Handler.Users;
using BusinessLayer.Queries.Query.Users;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.Commands;
using BusinessLayer.Commands.Handler.Users;
using BusinessLayer.Commands.Command.Users;

namespace ProjectStructure.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        // FIELDS
        IQueryProcessor queryProcessor;
        ICommandProcessor commandProcessor;

        // CONSTRUCTORS
        public UsersController(IQueryProcessor queryProcessor, ICommandProcessor commandProcessor)
        {
            this.queryProcessor = queryProcessor;
            this.commandProcessor = commandProcessor;
        }

        // ACTIONS
        // GET api/users
        [HttpGet]
        public Task<IEnumerable<User>> Get()
        {
            return queryProcessor.ProcessAsync<UserQueryHandler, AllUsersQuery, IEnumerable<User>>(new AllUsersQuery());
        }

        // GET api/users/{userId}
        [HttpGet("{userId}")]
        public Task<User> Get(int userId)
        {
            return queryProcessor.ProcessAsync<UserQueryHandler, SingleUserQuery, User>(new SingleUserQuery(userId));
        }

        // GET api/users/ordered
        [HttpGet]
        [Route("ordered")]
        public Task<IEnumerable<UserTasksDTO>> GetOrderedUsers()
        {
            return queryProcessor.ProcessAsync<UserQueryHandler, OrderedUserQuery, IEnumerable<UserTasksDTO>>(new OrderedUserQuery());
        }
        
        // POST: api/users
        [HttpPost]
        public Task<CommandResponse> Post([FromBody] Core.DataTransferObjects.User.CreateUserDTO value)
        {
            return commandProcessor.ProcessAsync<UserCommandHandler, CreateUserCommand, CommandResponse>(new CreateUserCommand(value, HttpContext.RequestServices));
        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        public Task<CommandResponse> Delete(int id)
        {
            return commandProcessor.ProcessAsync<UserCommandHandler, DeleteUserCommand, CommandResponse>(new DeleteUserCommand(id));
        }
    }
}
