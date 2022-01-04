using System.Collections.Generic;

using System.Threading.Tasks;

using TaskDb = DataAccessLayer.Entities.Task;

namespace Client.Interfaces
{
    public interface ITaskService
    {
        Task<TaskDb> GetAsync(int id);
        Task<IEnumerable<TaskDb>> GetAsync();

        Task<IEnumerable<TaskDb>> GetTasksWithShortNameAsync(int userId);
        Task<IEnumerable<TaskDb>> GetFinishedTasksAsync(int userId);

        Task<TaskDb> GetLongestTaskAsync(int userId);
        Task<TaskDb> GetTaskWithLongestDescriptionAsync(int projectId);
        Task<TaskDb> GetTaskWithShortestNameAsync(int projectId);

        Task<int> CountTaskAsync(int userId, int projectId);
        Task<int> UnfinishedTaskAmountAsync(int userId);

        Task<BusinessLayer.Commands.CommandResponse> DeleteTaskAsync(int id);
        Task<BusinessLayer.Commands.CommandResponse> CreateTaskAsync(Core.DataTransferObjects.Task.CreateTaskDTO createTaskDto);
    }
}
