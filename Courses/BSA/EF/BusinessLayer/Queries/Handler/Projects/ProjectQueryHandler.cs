using BusinessLayer.Interfaces;
using BusinessLayer.Queries.Query.Projects;

using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;

using System.Linq;
using System.Collections.Generic;

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
        public IEnumerable<Project> Handle(AllProjectQuery query)
        {
            return unitOfWork.GetRepository<Project, ProjectRepository>().Get();
        }

        public Project Handle(SingleProjectQuery query)
        {
            return unitOfWork.GetRepository<Project, ProjectRepository>().Get(query.Id);
        }

        public IDictionary<string, int> Handle(TasksAmountPerProjectQuery query)
        {
            return (from t in unitOfWork.GetRepository<Task, TaskRepository>().Get()
                     group t by t.ProjectId into tp
                     join p in unitOfWork.GetRepository<Project, ProjectRepository>().Get() on tp.Key equals p.Id
                     where p.AuthorId == query.UserId
                     select new
                     {
                         Project = p,
                         TaskAmount = tp.Count()
                     })
                    .ToDictionary(k => k.Project.ToString(), v => v.TaskAmount);
        }

        public Project Handle(LastProjectQuery query)
        {
            return unitOfWork.GetRepository<Project, ProjectRepository>().GetLastProject(query.UserId);
        }
    }
}
