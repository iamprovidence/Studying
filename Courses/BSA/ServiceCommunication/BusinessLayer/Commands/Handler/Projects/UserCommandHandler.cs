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
            if (unitOfWork.GetRepository<Project, ProjectRepository>().Delete(command.ProjectId))
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
                Message = "Could not delete project. Check id"
            };
        }

        public CommandResponse Execute(CreateProjectCommand command)
        {
            // map project
            ProjectRepository projectRepository = unitOfWork.GetRepository<Project, ProjectRepository>();
            Project project = command.ServiceProvider.GetService<IMapper>()
                        .Map<Project>(command.CreateProjectDTO);
            project.Id = projectRepository.MaxId() + 1;

            // insert
            if (projectRepository.Insert(project))
            {
                return new CommandResponse
                {
                    IsSucessed = true,
                    Message = $"New project added. Project id = {project.Id}"
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
