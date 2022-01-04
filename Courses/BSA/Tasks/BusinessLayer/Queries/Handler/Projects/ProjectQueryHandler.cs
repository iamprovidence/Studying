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

namespace BusinessLayer.Queries.Handler.Projects
{
    public class ProjectQueryHandler : 
        IQueryHandler<AllProjectQuery, IEnumerable<Project>>,
        IQueryHandler<SingleProjectQuery, Project>,
        IQueryHandler<LastProjectQuery, Project>,
        IQueryHandler<TasksAmountPerProjectQuery, IDictionary<string, int>>,
        IUnitOfWorkSettable
    {
        // FIELDS
        IUnitOfWork unitOfWork;

        // COMSTRUCTORS
        public ProjectQueryHandler()
        {
            unitOfWork = null;
        }
        public void SetUnitOfWork(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // METHODS
        public Task<IEnumerable<Project>> HandleAsync(AllProjectQuery query)
        {
            return unitOfWork.GetRepository<Project, ProjectRepository>().GetAsync();
        }

        public Task<Project> HandleAsync(SingleProjectQuery query)
        {
            return unitOfWork.GetRepository<Project, ProjectRepository>().GetAsync(query.Id);
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
                         Project = p,
                         TaskAmount = tp.Count()
                     })
                    .ToDictionary(k => k.Project.ToString(), v => v.TaskAmount);
        }

        public Task<Project> HandleAsync(LastProjectQuery query)
        {
            return unitOfWork.GetRepository<Project, ProjectRepository>().GetLastProjectAsync(query.UserId);
        }
    }
}
