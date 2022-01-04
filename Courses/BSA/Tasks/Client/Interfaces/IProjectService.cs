using BusinessLayer.Commands;

using Core.DataTransferObjects.Project;

using DataAccessLayer.Entities;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Interfaces
{
    public interface IProjectService
    {
        Task<Project> GetProjectAsync(int id);
        Task<IEnumerable<Project>> GetAllAsync();
        Task<IDictionary<string, int>> GetTasksAmountPerProjectAsync(int userId);
        Task<Project> GetLastProjectAsync(int userId);

        Task<CommandResponse> CreateAsync(CreateProjectDTO createProjectDTO);

        Task<CommandResponse> DeleteAsync(int id);
    }
}
