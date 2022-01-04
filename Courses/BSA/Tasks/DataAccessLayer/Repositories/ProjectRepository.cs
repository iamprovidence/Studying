using DataAccessLayer.Entities;

using System.Linq;
using System.Threading.Tasks;

using Task = System.Threading.Tasks.Task;

namespace DataAccessLayer.Repositories
{
    public class ProjectRepository : RepositoryBase<Project>, Interfaces.Repositories.IProjectRepository
    {
        // METHODS
        public virtual Task<Project> GetLastProjectAsync(int userId)
        {
            return Task.Run(() =>
                entities
                        .Where(p => p.AuthorId == userId)
                        .OrderByDescending(p => p.CreatedAt)
                        .FirstOrDefault());
        }
    }
}
