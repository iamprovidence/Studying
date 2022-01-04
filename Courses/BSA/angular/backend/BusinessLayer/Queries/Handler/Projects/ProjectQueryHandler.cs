using BusinessLayer.Interfaces;
using BusinessLayer.Queries.Query.Projects;

using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;

using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Task = System.Threading.Tasks.Task;
using TaskDb = DataAccessLayer.Entities.Task;

using AutoMapper;

using Core.DataTransferObjects.Project;

namespace BusinessLayer.Queries.Handler.Projects
{
    public class ProjectQueryHandler : 
        IQueryHandler<AllProjectQuery, IEnumerable<ProjectListDTO>>,
        IQueryHandler<SingleProjectQuery, ProjectViewDTO>,
        IQueryHandler<LastProjectQuery, Project>,
        IQueryHandler<TasksAmountPerProjectQuery, IDictionary<string, int>>,
        IUnitOfWorkSettable, IMapperSettable
    {
        // FIELDS
        IMapper mapper;
        IUnitOfWork unitOfWork;

        // COMSTRUCTORS
        public ProjectQueryHandler()
        {
            mapper = null;
            unitOfWork = null;
        }
        public void SetUnitOfWork(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void SetMapper(IMapper mapper)
        {
            this.mapper = mapper;
        }

        // METHODS
        public async Task<IEnumerable<ProjectListDTO>> HandleAsync(AllProjectQuery query)
        {
            IEnumerable<Project> projects = await unitOfWork.GetRepository<Project, ProjectRepository>().GetAsync();

            return mapper.Map<ProjectListDTO[]>(projects);
        }

        public async Task<ProjectViewDTO> HandleAsync(SingleProjectQuery query)
        {
            Project project = await unitOfWork.GetRepository<Project, ProjectRepository>().GetAsync(query.Id, 
                    includeProperties: $"{nameof(Project.Author)}, {nameof(Project.Team)}");

            return mapper.Map<ProjectViewDTO>(project);
        }

        public async Task<IDictionary<string, int>> HandleAsync(TasksAmountPerProjectQuery query)
        {
            IEnumerable<TaskDb> tasksList = await unitOfWork.GetRepository<TaskDb, TaskRepository>().GetAsync();
            IEnumerable<Project> projectList = await unitOfWork.GetRepository<Project, ProjectRepository>().GetAsync();            

            return (from t in tasksList
                     group t by t.ProjectId into tp
                     join p in projectList on tp.Key equals p.Id
                     where p.AuthorId == query.UserId
                     select new
                     {
                         ProjectName = p.Name,
                         TaskAmount = tp.Count()
                     })
                    .ToDictionary(k => k.ProjectName, v => v.TaskAmount);
        }

        public Task<Project> HandleAsync(LastProjectQuery query)
        {
            return unitOfWork.GetRepository<Project, ProjectRepository>().GetLastProjectAsync(query.UserId);
        }

    }
}
