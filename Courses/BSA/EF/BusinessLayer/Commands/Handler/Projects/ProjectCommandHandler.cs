using AutoMapper;

using Microsoft.Extensions.DependencyInjection;

using BusinessLayer.Commands.Command.Projects;

using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;

namespace BusinessLayer.Commands.Handler.Users
{
    public class ProjectCommandHandler :
        Interfaces.ICommandHandler<DeleteProjectCommand, CommandResponse>,
        Interfaces.ICommandHandler<CreateProjectCommand, CommandResponse>,
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
        public CommandResponse Execute(DeleteProjectCommand command)
        {
            bool isDeleted = false;
            string message = "Deleted";
            try
            {
                isDeleted = unitOfWork.GetRepository<Project, ProjectRepository>().Delete(command.ProjectId);
                if (!isDeleted) message = "Could not delete";

                unitOfWork.Save();
            }
            catch (System.Exception e)
            {
                isDeleted = false;
                message = Common.Algorithms.GetFullText(e);
            }

            // result
            return new CommandResponse
            {
                IsSucessed = isDeleted,
                Message = message
            };
        }

        public CommandResponse Execute(CreateProjectCommand command)
        {
            // map project
            ProjectRepository projectRepository = unitOfWork.GetRepository<Project, ProjectRepository>();
            Project project = command.ServiceProvider.GetService<IMapper>()
                        .Map<Project>(command.CreateProjectDTO);

            // insert
            bool isInserted = false;
            string message = "Project created";
            try
            {
                isInserted = projectRepository.Insert(project);
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
