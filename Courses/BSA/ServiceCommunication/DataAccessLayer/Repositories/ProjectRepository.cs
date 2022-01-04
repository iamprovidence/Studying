using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;

using System.Linq;

namespace DataAccessLayer.Repositories
{
    public class ProjectRepository : RepositoryBase<Project>, Interfaces.Repositories.IProjectRepository
    {
        // CONSTRUCTORS
        public override void SetDataProvider(IDataProvider dataProvider)
        {
            entities = new System.Collections.Generic.List<Project>(dataProvider.Projects);
        }

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
