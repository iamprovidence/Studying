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

using Core.DataTransferObjects.User;

namespace WebApi.Controllers
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
        public Task<IEnumerable<UserListDTO>> Get()
        {
            return queryProcessor.ProcessAsync<UserQueryHandler, AllUsersQuery, IEnumerable<UserListDTO>>(new AllUsersQuery());
        }

        // GET api/users/{userId}
        [HttpGet("{userId}")]
        public Task<UserViewDTO> Get(int userId)
        {
            return queryProcessor.ProcessAsync<UserQueryHandler, SingleUserQuery, UserViewDTO>(new SingleUserQuery(userId));
        }

        // GET api/users/ordered
        [HttpGet]
        [Route("ordered")]
        public Task<IEnumerable<UserTasksDTO>> GetOrderedUsers()
        {
            return queryProcessor.ProcessAsync<UserQueryHandler, OrderedUserQuery, IEnumerable<UserTasksDTO>>(new OrderedUserQuery());
        }


        // PATCH: api/users
        [HttpPatch]
        public Task<CommandResponse> Patch([FromBody] UpdateUserDTO value)
        {
            return commandProcessor.ProcessAsync<UserCommandHandler, UpdateUserCommand, CommandResponse>(new UpdateUserCommand(value));
        }

        // POST: api/users
        [HttpPost]
        public Task<CommandResponse> Post([FromBody] CreateUserDTO value)
        {
            return commandProcessor.ProcessAsync<UserCommandHandler, CreateUserCommand, CommandResponse>(new CreateUserCommand(value));
        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        public Task<CommandResponse> Delete(int id)
        {
            return commandProcessor.ProcessAsync<UserCommandHandler, DeleteUserCommand, CommandResponse>(new DeleteUserCommand(id));
        }
    }
}
