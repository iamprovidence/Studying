using AutoMapper;

using Microsoft.Extensions.DependencyInjection;

using BusinessLayer.Commands.Command.Users;

using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;

namespace BusinessLayer.Commands.Handler.Users
{
    public class UserCommandHandler :
        Interfaces.ICommandHandler<DeleteUserCommand, CommandResponse>,
        Interfaces.ICommandHandler<CreateUserCommand, CommandResponse>,
        Interfaces.IUnitOfWorkSettable
    {
        // FIELSD
        IUnitOfWork unitOfWork;

        // CONSTRUCTORS
        public void SetUnitOfWork(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // METHODS        
        public CommandResponse Execute(DeleteUserCommand command)
        {
            if (unitOfWork.GetRepository<User, UserRepository>().Delete(command.UserId))
            {
                return new CommandResponse
                {
                    IsSucessed = true,
                    Message = "Deleted"
                };
            }
            else return new CommandResponse
            {
                IsSucessed = false,
                Message = "Could not delete user. Check id"
            };
        }

        public CommandResponse Execute(CreateUserCommand command)
        {
            // map user
            UserRepository userRepository = unitOfWork.GetRepository<User, UserRepository>();
            User user = command.ServiceProvider.GetService<IMapper>()
                        .Map<User>(command.CreateUserDTO);
            user.Id = userRepository.MaxId() + 1;

            // insert
            if (userRepository.Insert(user))
            {
                return new CommandResponse
                {
                    IsSucessed = true,
                    Message = $"New user added. User id = {user.Id}"
                };
            }
            else
            {
                return new CommandResponse
                {
                    IsSucessed = false,
                    Message = "Could not add new item",
                };
            }
        }
    }
}
