using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;

using System.Linq;

namespace DataAccessLayer.Repositories
{
    public class TaskRepository : RepositoryBase<Task>, Interfaces.Repositories.ITaskRepository
    {        
        // CONSTRUCTORS
        public override void SetDataProvider(IDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;
            this.entities = new System.Collections.Generic.List<Task>(dataProvider.Tasks);
        }

        // METHODS
        public virtual int Count(int userId, int projectId)
        {
            return entities
                    .Where(t => t.PerformerId == userId)
                    .Where(t => t.ProjectId == projectId)
                    .Count();
        }
        public virtual int CountExcept(int userId)
        {
            int finishedStatusId = dataProvider.TaskStates.First(s => s.Value == "Finished").Id;

            return CountExcept(userId, finishedStatusId);
        }
        public virtual int CountExcept(int userId, int taskStatus)
        {
            return entities
                        .Where(t => t.PerformerId == userId)
                        .Where(t => t.StateId != taskStatus)
                        .Count();
        }

        public virtual Task GetWithLongestDescription(int projectId)
        {
            return entities
                        .Where(t => t.ProjectId == projectId)
                        .OrderByDescending(t => t.Description.Length)
                        .FirstOrDefault();
        }

        public virtual Task GetWithShortestName(int projectId)
        {
            return entities
                        .Where(t => t.ProjectId == projectId)
                        .OrderBy(t => t.Name.Length)
                        .FirstOrDefault();
        }

        public virtual Task GetLongestTask(int userId)
        {
            return entities
                        .Where(t => t.PerformerId == userId)
                        .OrderByDescending(t => t.FinishedAt - t.CreatedAt)
                        .FirstOrDefault();
        }
        
    }
}
