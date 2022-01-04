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
            // deleting
            bool isDeleted = false;
            string messsage = "Deleted";

            try
            {
                isDeleted = unitOfWork.GetRepository<User, UserRepository>().Delete(command.UserId);
                if (!isDeleted) messsage = "Could not delete";
                unitOfWork.Save();
            }
            catch (System.Exception e)
            {
                isDeleted = false;
                messsage = Common.Algorithms.GetFullText(e);
            }

            // result
            return new CommandResponse
            {
                IsSucessed = isDeleted,
                Message = messsage
            };
        }

        public CommandResponse Execute(CreateUserCommand command)
        {
            // map user
            UserRepository userRepository = unitOfWork.GetRepository<User, UserRepository>();
            User user = command.ServiceProvider.GetService<IMapper>()
                        .Map<User>(command.CreateUserDTO);

            // insert
            bool isInserted = false;
            string message = "New user created";

            try
            {
                isInserted = userRepository.Insert(user);
                unitOfWork.Save();
            }
            catch (System.Exception e)
            {
                isInserted = false;
                message = Common.Algorithms.GetFullText(e);
            }

            // result
            return new CommandResponse
            {
                IsSucessed = isInserted,
                Message = message
            };
        }
    }
}
