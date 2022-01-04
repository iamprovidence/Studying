using BusinessLayer.Interfaces;
using BusinessLayer.Queries.Query.Tasks;

using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;

using System.Collections.Generic;

namespace BusinessLayer.Queries.Handler.Tasks
{
    // I guess handlers should be separated and also this look ugly
    // but I am too tired, so... yeah >_<
    public class TaskQueryHandler : 
        IQueryHandler<ShortTaskQuery, IEnumerable<Task>>,
        IQueryHandler<FinishedInYearQuery, IEnumerable<Task>>,
        IQueryHandler<AllTaskQuery, IEnumerable<Task>>,
        IQueryHandler<TaskWithLongestDescriptionQuery, Task>,
        IQueryHandler<TaskWithShortestNameQuery, Task>,
        IQueryHandler<LongestTaskQuery, Task>,
        IQueryHandler<CountUnfinishedTaskQuery, int>,
        IQueryHandler<SingleTaskQuery, Task>,
        IQueryHandler<CountTaskPerProjectQuery, int>,
        IUnitOfWorkSettable
    {
        // FIELDS
        IUnitOfWork unitOfWork;

        // CONSTRUCTORS
        public TaskQueryHandler()
        {
            unitOfWork = null;
        }
        public void SetUnitOfWork(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // METHODS
        public IEnumerable<Task> Handle(ShortTaskQuery query)
        {
            return unitOfWork.GetRepository<Task, TaskRepository>()
                                .Get(filter: t => t.PerformerId == query.UserId && 
                                             t.Name.Length < query.MaxTaskNameLength);
        }

        public IEnumerable<Task> Handle(FinishedInYearQuery query)
        {
            return unitOfWork.GetRepository<Task, TaskRepository>()
                                .Get(filter: t => t.PerformerId == query.UserId &&
                                             t.FinishedAt.Year == query.Year);
        }

        public IEnumerable<Task> Handle(AllTaskQuery query)
        {
            return unitOfWork.GetRepository<Task, TaskRepository>().Get();
        }

        public Task Handle(TaskWithLongestDescriptionQuery query)
        {
            return unitOfWork.GetRepository<Task, TaskRepository>().GetWithLongestDescription(query.ProjectId);
        }

        public Task Handle(TaskWithShortestNameQuery query)
        {
            return unitOfWork.GetRepository<Task, TaskRepository>().GetWithShortestName(query.ProjectId);
        }

        public Task Handle(LongestTaskQuery query)
        {
            return unitOfWork.GetRepository<Task, TaskRepository>().GetLongestTask(query.UserId);
        }

        public int Handle(CountUnfinishedTaskQuery query)
        {
            return unitOfWork.GetRepository<Task, TaskRepository>().CountExcept(query.UserId);
        }

        public Task Handle(SingleTaskQuery query)
        {
            return unitOfWork.GetRepository<Task, TaskRepository>().Get(query.Id);
        }

        public int Handle(CountTaskPerProjectQuery query)
        {
            return unitOfWork.GetRepository<Task, TaskRepository>().Count(query.UserId, query.ProjectId);
        }
    }
}
