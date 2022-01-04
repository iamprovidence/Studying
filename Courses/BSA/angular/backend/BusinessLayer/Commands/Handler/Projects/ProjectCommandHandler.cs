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
        Interfaces.ICommandHandler<UpdateProjectCommand, CommandResponse>,
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

        public async Task<CommandResponse> ExecuteAsync(UpdateProjectCommand command)
        {
            // map project
            Project entityToUpdate = mapper.Map<Project>(command.UpdateProjectDTO);

            // update
            bool isUpdated = true;
            string message = "Record updated";

            try
            {
                unitOfWork.Update(entityToUpdate, excludeProperties: $"{nameof(Project.CreatedAt)}");
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
