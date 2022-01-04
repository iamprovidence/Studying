using AutoMapper;

using BusinessLayer.Commands.Command.Users;

using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;

using System.Threading.Tasks;

namespace BusinessLayer.Commands.Handler.Users
{
    public class UserCommandHandler :
        Interfaces.ICommandHandler<DeleteUserCommand, CommandResponse>,
        Interfaces.ICommandHandler<CreateUserCommand, CommandResponse>,
        Interfaces.ICommandHandler<UpdateUserCommand, CommandResponse>,
        Interfaces.IUnitOfWorkSettable, Interfaces.IMapperSettable
    {
        // FIELDS
        IMapper mapper;
        IUnitOfWork unitOfWork;

        // CONSTRUCTORS
        public void SetUnitOfWork(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void SetMapper(IMapper mapper)
        {
            this.mapper = mapper;
        }

        // METHODS        
        public async Task<CommandResponse> ExecuteAsync(DeleteUserCommand command)
        {
            // deleting
            bool isDeleted = false;
            string messsage = "Deleted";

            try
            {
                isDeleted = await unitOfWork.GetRepository<User, UserRepository>().DeleteAsync(command.UserId);
                if (!isDeleted) messsage = "Could not delete";
                await unitOfWork.SaveAsync();
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

        public async Task<CommandResponse> ExecuteAsync(CreateUserCommand command)
        {
            // map user
            UserRepository userRepository = unitOfWork.GetRepository<User, UserRepository>();
            User user = mapper.Map<User>(command.CreateUserDTO);

            // insert
            bool isInserted = false;
            string message = "New user created";

            try
            {
                isInserted = await userRepository.InsertAsync(user);
                await unitOfWork.SaveAsync();
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

        public async Task<CommandResponse> ExecuteAsync(UpdateUserCommand command)
        {
            // map user
            User user = mapper.Map<User>(command.UpdateUserDTO);

            // update
            bool isUpdated = true;
            string message = "Record updated";

            try
            {
                unitOfWork.Update(user, excludeProperties: $"{nameof(User.RegisteredAt)}");
                await unitOfWork.SaveAsync();
            }
            catch (System.Exception e)
            {
                isUpdated = false;
                message = Common.Algorithms.GetFullText(e);
            }

            // result
            return new CommandResponse
            {
                IsSucessed = isUpdated,
                Message = message
            };
        }
    }
}
