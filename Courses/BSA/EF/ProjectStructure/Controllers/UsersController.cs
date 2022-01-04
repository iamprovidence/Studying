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
        public IEnumerable<User> Get()
        {
            return queryProcessor.Process<UserQueryHandler, AllUsersQuery, IEnumerable<User>>(new AllUsersQuery());
        }

        // GET api/users/{userId}
        [HttpGet("{userId}")]
        public User Get(int userId)
        {
            return queryProcessor.Process<UserQueryHandler, SingleUserQuery, User>(new SingleUserQuery(userId));
        }

        // GET api/users/ordered
        [HttpGet]
        [Route("ordered")]
        public IEnumerable<UserTasksDTO> GetOrderedUsers()
        {
            return queryProcessor.Process<UserQueryHandler, OrderedUserQuery, IEnumerable<UserTasksDTO>>(new OrderedUserQuery());
        }
        
        // POST: api/users
        [HttpPost]
        public CommandResponse Post([FromBody] Core.DataTransferObjects.User.CreateUserDTO value)
        {
            return commandProcessor.Process<UserCommandHandler, CreateUserCommand, CommandResponse>(new CreateUserCommand(value, HttpContext.RequestServices));
        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        public CommandResponse Delete(int id)
        {
            return commandProcessor.Process<UserCommandHandler, DeleteUserCommand, CommandResponse>(new DeleteUserCommand(id));
        }
    }
}
