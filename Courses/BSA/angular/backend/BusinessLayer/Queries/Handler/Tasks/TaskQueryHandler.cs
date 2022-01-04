using BusinessLayer.Interfaces;
using BusinessLayer.Queries.Query.Tasks;

using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;
using DataAccessLayer.Entities;

using System.Threading.Tasks;
using System.Collections.Generic;

using AutoMapper;


using Core.DataTransferObjects.Task;
using Core.DataTransferObjects.TaskState;

using TaskDb = DataAccessLayer.Entities.Task;

namespace BusinessLayer.Queries.Handler.Tasks
{
    // I guess handlers should be separated and also this look ugly
    // but I am too tired, so... yeah >_<
    public class TaskQueryHandler : 
        IQueryHandler<AllTaskQuery, IEnumerable<TaskListDTO>>,
        IQueryHandler<AllStateQuery, IEnumerable<TaskStateListDTO>>,
        IQueryHandler<SingleTaskQuery, TaskViewDTO>,
        IQueryHandler<ShortTaskQuery, IEnumerable<TaskDb>>,
        IQueryHandler<FinishedInYearQuery, IEnumerable<TaskDb>>,
        IQueryHandler<TaskWithLongestDescriptionQuery, TaskDb>,
        IQueryHandler<TaskWithShortestNameQuery, TaskDb>,
        IQueryHandler<LongestTaskQuery, TaskDb>,
        IQueryHandler<CountUnfinishedTaskQuery, int>,
        IQueryHandler<CountTaskPerProjectQuery, int>,
        IUnitOfWorkSettable, IMapperSettable
    {
        // FIELDS
        IMapper mapper;
        IUnitOfWork unitOfWork;

        // CONSTRUCTORS
        public TaskQueryHandler()
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

        public async Task<IEnumerable<TaskListDTO>> HandleAsync(AllTaskQuery query)
        {
            IEnumerable<TaskDb> tasks = await unitOfWork.GetRepository<TaskDb, TaskRepository>().GetAsync();

            return mapper.Map<TaskListDTO[]>(tasks);
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

        public async Task<TaskViewDTO> HandleAsync(SingleTaskQuery query)
        {
            TaskDb task = await unitOfWork.GetRepository<TaskDb, TaskRepository>().GetAsync(query.Id,
                includeProperties: $"{nameof(TaskDb.Performer)}, {nameof(TaskDb.Project)}, {nameof(TaskDb.TaskState)}");

            return mapper.Map<TaskViewDTO>(task);
        }

        public Task<int> HandleAsync(CountTaskPerProjectQuery query)
        {
            return unitOfWork.GetRepository<TaskDb, TaskRepository>().CountAsync(query.UserId, query.ProjectId);
        }

        public async Task<IEnumerable<TaskStateListDTO>> HandleAsync(AllStateQuery query)
        {
            IEnumerable<TaskState> states = await unitOfWork.GetRepository<TaskState, RepositoryBase<TaskState>>().GetAsync();

            return mapper.Map<TaskStateListDTO[]>(states);
        }
    }
}
