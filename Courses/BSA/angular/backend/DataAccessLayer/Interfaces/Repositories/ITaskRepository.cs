using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces.Repositories
{
    public interface ITaskRepository : IRepository<Entities.Task>
    {
        Task<int> CountAsync(int userId, int projectId);
        Task<int> CountExceptAsync(int userId);
        Task<int> CountExceptAsync(int userId, int taskStatus);

        Task<Entities.Task> GetWithLongestDescriptionAsync(int projectId);
        Task<Entities.Task> GetWithShortestNameAsync(int projectId);
        Task<Entities.Task> GetLongestTaskAsync(int userId);
    }
}
