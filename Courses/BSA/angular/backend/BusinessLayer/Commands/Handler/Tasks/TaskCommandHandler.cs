using AutoMapper;

using BusinessLayer.Commands.Command.Tasks;

using DataAccessLayer.Interfaces;
using DataAccessLayer.Interfaces.Repositories;
using DataAccessLayer.Repositories;

using Microsoft.Extensions.DependencyInjection;

using System.Threading.Tasks;

using TaskDb = DataAccessLayer.Entities.Task;

namespace BusinessLayer.Commands.Handler.Tasks
{
    public class TaskCommandHandler : 
        Interfaces.ICommandHandler<CreateTaskCommand, CommandResponse>,
        Interfaces.ICommandHandler<DeleteTaskCommand, CommandResponse>,
        Interfaces.ICommandHandler<UpdateTaskCommand, CommandResponse>,
        Interfaces.IUnitOfWorkSettable, Interfaces.IMapperSettable
    {
        // FIELDS
        IMapper mapper;
        IUnitOfWork unitOfWork;
        ITaskRepository taskRepository;

        // CONSTRUCTORS
        public void SetUnitOfWork(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.taskRepository = unitOfWork.GetRepository<TaskDb, TaskRepository>();
        }

        public void SetMapper(IMapper mapper)
        {
            this.mapper = mapper;
        }

        // METHODS
        public async Task<CommandResponse> ExecuteAsync(CreateTaskCommand command)
        {
            // map task
            TaskDb task = command.ServiceProvider.GetService<IMapper>()
                        .Map<TaskDb>(command.CreateTaskDTO);

            // insert
            bool insertingStatus = false;
            string message = "New task added";

            try
            {
                insertingStatus = await taskRepository.InsertAsync(task);
                await unitOfWork.SaveAsync();
            }
            catch (System.Exception e)
            {
                insertingStatus = false;
                message = Common.Algorithms.GetFullText(e);
            }

            // result
            return new CommandResponse
            {
                IsSucessed = insertingStatus,
                Message = message
            };
        }

        public async Task<CommandResponse> ExecuteAsync(DeleteTaskCommand command)
        {
            // delete
            bool deletingStatus = false;
            string message = "Task deleted";
            try
            {
                deletingStatus = await taskRepository.DeleteAsync(command.TaskId);
                await unitOfWork.SaveAsync();
                if (!deletingStatus) message = "Could not delete task";
            }
            catch (System.Exception e)
            {
                deletingStatus = false;
                message = Common.Algorithms.GetFullText(e);
            }


            // result
            return new CommandResponse
            {
                IsSucessed = deletingStatus,
                Message = message
            };
        }

        public async Task<CommandResponse> ExecuteAsync(UpdateTaskCommand command)
        {
            // map task
            TaskDb entityToUpdate = mapper.Map<TaskDb>(command.UpdateTaskDTO);

            // update
            bool isUpdated = true;
            string message = "Record updated";

            try
            {
                unitOfWork.Update(entityToUpdate, excludeProperties: $"{nameof(TaskDb.CreatedAt)}");
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
