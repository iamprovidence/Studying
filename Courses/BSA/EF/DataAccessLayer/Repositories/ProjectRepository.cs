using DataAccessLayer.Entities;

using System.Linq;

namespace DataAccessLayer.Repositories
{
    public class ProjectRepository : RepositoryBase<Project>, Interfaces.Repositories.IProjectRepository
    {
        // METHODS
        public virtual Project GetLastProject(int userId)
        {
            return entities
                        .Where(p => p.AuthorId == userId)
                        .OrderByDescending(p => p.CreatedAt)
                        .FirstOrDefault();
        }
    }
}
