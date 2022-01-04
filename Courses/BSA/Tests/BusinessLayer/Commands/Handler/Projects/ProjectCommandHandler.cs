using AutoMapper;

using Microsoft.Extensions.DependencyInjection;

using BusinessLayer.Commands.Command.Projects;

using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;

using System.Threading.Tasks;

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
        public async Task<CommandResponse> ExecuteAsync(DeleteProjectCommand command)
        {
            bool isDeleted = false;
            string message = "Deleted";
            try
            {
                isDeleted = await unitOfWork.GetRepository<Project, ProjectRepository>().DeleteAsync(command.ProjectId);
                if (!isDeleted) message = "Could not delete";

                await unitOfWork.SaveAsync();
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

        public async Task<CommandResponse> ExecuteAsync(CreateProjectCommand command)
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
                isInserted = await projectRepository.InsertAsync(project);
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
    }
}
