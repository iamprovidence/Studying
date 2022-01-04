using DataAccessLayer.Entities;

using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Threading.Tasks;

using Task = System.Threading.Tasks.Task;

namespace DataAccessLayer.Repositories
{
    public class ProjectRepository : RepositoryBase<Project>, Interfaces.Repositories.IProjectRepository
    {
        // CONSTRUCTORS
        public ProjectRepository() 
            : base() { }

        public ProjectRepository(DbContext dbContext)
            : base(dbContext) { }

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
