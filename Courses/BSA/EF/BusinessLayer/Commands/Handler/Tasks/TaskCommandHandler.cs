using AutoMapper;

using BusinessLayer.Commands.Command.Tasks;

using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Interfaces.Repositories;
using DataAccessLayer.Repositories;

using Microsoft.Extensions.DependencyInjection;

namespace BusinessLayer.Commands.Handler.Tasks
{
    public class TaskCommandHandler : 
        Interfaces.ICommandHandler<CreateTaskCommand, CommandResponse>,
        Interfaces.ICommandHandler<DeleteTaskCommand, CommandResponse>,
        Interfaces.IUnitOfWorkSettable
    {
        // FIELDS
        IUnitOfWork unitOfWork;
        ITaskRepository taskRepository;

        // CONSTRUCTORS
        public void SetUnitOfWork(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.taskRepository = unitOfWork.GetRepository<Task, TaskRepository>();
        }

        // METHODS
        public CommandResponse Execute(CreateTaskCommand command)
        {
            // map task
            Task task = command.ServiceProvider.GetService<IMapper>()
                        .Map<Task>(command.CreateTaskDTO);

            // insert
            bool insertingStatus = false;
            string message = "New task added";

            try
            {
                insertingStatus = taskRepository.Insert(task);
                unitOfWork.Save();
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

        public CommandResponse Execute(DeleteTaskCommand command)
        {
            // delete
            bool deletingStatus = false;
            string message = "Task deleted";
            try
            {
                deletingStatus = taskRepository.Delete(command.TaskId);
                unitOfWork.Save();
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
    }
}
