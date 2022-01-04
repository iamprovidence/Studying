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
            task.Id = taskRepository.MaxId() + 1;

            // insert
            if (taskRepository.Insert(task))
            {
                return new CommandResponse
                {
                    IsSucessed = true,
                    Message = $"New task added. Task id = {task.Id}"
                };
            }
            else
            {
                return new CommandResponse
                {
                    IsSucessed = false,
                    Message = "Could not add new task",
                };
            }
        }

        public CommandResponse Execute(DeleteTaskCommand command)
        {
            // delete
            if (taskRepository.Delete(command.TaskId)) return new CommandResponse
            {
                IsSucessed = true,
                Message = "Task deleted"
            };
            else return new CommandResponse
            {
                IsSucessed = false,
                Message = "Could not delete task. Check id"
            };
        }
    }
}
