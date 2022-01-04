using System.Linq;
using System.Threading.Tasks;

using Task = System.Threading.Tasks.Task;
using TaskDb = DataAccessLayer.Entities.Task;

namespace DataAccessLayer.Repositories
{
    public class TaskRepository : RepositoryBase<TaskDb>, Interfaces.Repositories.ITaskRepository
    {        
        // METHODS
        public virtual Task<int> CountAsync(int userId, int projectId)
        {
            return Task.Run(() =>  
                entities
                    .Where(t => t.PerformerId == userId)
                    .Where(t => t.ProjectId == projectId)
                    .Count());
        }
        public virtual Task<int> CountExceptAsync(int userId)
        {
            int finishedStatusId = dbContext.Set<Entities.TaskState>().First(s => s.Value == "Finished").Id;

            return CountExceptAsync(userId, finishedStatusId);
        }
        public virtual Task<int> CountExceptAsync(int userId, int taskStatus)
        {
            return Task.Run(() =>
                    entities
                        .Where(t => t.PerformerId == userId)
                        .Where(t => t.TaskStateId != taskStatus)
                        .Count());
        }

        public virtual Task<TaskDb> GetWithLongestDescriptionAsync(int projectId)
        {
            return Task.Run(() =>
               entities
                        .Where(t => t.ProjectId == projectId)
                        .OrderByDescending(t => t.Description.Length)
                        .FirstOrDefault());
        }

        public virtual Task<TaskDb> GetWithShortestNameAsync(int projectId)
        {
            return Task.Run(() =>
                entities
                        .Where(t => t.ProjectId == projectId)
                        .OrderBy(t => t.Name.Length)
                        .FirstOrDefault());
        }

        public virtual Task<TaskDb> GetLongestTaskAsync(int userId)
        {
            return Task.Run(() =>
               entities
                        .Where(t => t.PerformerId == userId)
                        .OrderByDescending(t => t.FinishedAt - t.CreatedAt)
                        .FirstOrDefault());
        }
        
    }
}
