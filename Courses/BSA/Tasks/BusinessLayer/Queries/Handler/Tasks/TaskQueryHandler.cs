using BusinessLayer.Interfaces;
using BusinessLayer.Queries.Query.Tasks;

using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;

using System.Threading.Tasks;
using System.Collections.Generic;

using TaskDb = DataAccessLayer.Entities.Task;

namespace BusinessLayer.Queries.Handler.Tasks
{
    // I guess handlers should be separated and also this look ugly
    // but I am too tired, so... yeah >_<
    public class TaskQueryHandler : 
        IQueryHandler<ShortTaskQuery, IEnumerable<TaskDb>>,
        IQueryHandler<FinishedInYearQuery, IEnumerable<TaskDb>>,
        IQueryHandler<AllTaskQuery, IEnumerable<TaskDb>>,
        IQueryHandler<TaskWithLongestDescriptionQuery, TaskDb>,
        IQueryHandler<TaskWithShortestNameQuery, TaskDb>,
        IQueryHandler<LongestTaskQuery, TaskDb>,
        IQueryHandler<CountUnfinishedTaskQuery, int>,
        IQueryHandler<SingleTaskQuery, TaskDb>,
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
        public Task<IEnumerable<TaskDb>> HandleAsync(ShortTaskQuery query)
        {
            return unitOfWork.GetRepository<TaskDb, TaskRepository>()
                                .GetAsync(filter: t => t.PerformerId == query.UserId && 
                                             t.Name.Length < query.MaxTaskNameLength);
        }

        public Task<IEnumerable<TaskDb>> HandleAsync(FinishedInYearQuery query)
        {
            return unitOfWork.GetRepository<TaskDb, TaskRepository>()
                                .GetAsync(filter: t => t.PerformerId == query.UserId &&
                                             t.FinishedAt.Year == query.Year);
        }

        public Task<IEnumerable<TaskDb>> HandleAsync(AllTaskQuery query)
        {
            return unitOfWork.GetRepository<TaskDb, TaskRepository>().GetAsync();
        }

        public Task<TaskDb> HandleAsync(TaskWithLongestDescriptionQuery query)
        {
            return unitOfWork.GetRepository<TaskDb, TaskRepository>().GetWithLongestDescriptionAsync(query.ProjectId);
        }

        public Task<TaskDb> HandleAsync(TaskWithShortestNameQuery query)
        {
            return unitOfWork.GetRepository<TaskDb, TaskRepository>().GetWithShortestNameAsync(query.ProjectId);
        }

        public Task<TaskDb> HandleAsync(LongestTaskQuery query)
        {
            return unitOfWork.GetRepository<TaskDb, TaskRepository>().GetLongestTaskAsync(query.UserId);
        }

        public Task<int> HandleAsync(CountUnfinishedTaskQuery query)
        {
            return unitOfWork.GetRepository<TaskDb, TaskRepository>().CountExceptAsync(query.UserId);
        }

        public Task<TaskDb> HandleAsync(SingleTaskQuery query)
        {
            return unitOfWork.GetRepository<TaskDb, TaskRepository>().GetAsync(query.Id);
        }

        public Task<int> HandleAsync(CountTaskPerProjectQuery query)
        {
            return unitOfWork.GetRepository<TaskDb, TaskRepository>().CountAsync(query.UserId, query.ProjectId);
        }
    }
}
