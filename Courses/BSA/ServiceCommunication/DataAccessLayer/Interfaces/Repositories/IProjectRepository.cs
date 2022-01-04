namespace DataAccessLayer.Interfaces.Repositories
{
    public interface IProjectRepository : IRepository<Entities.Project>
    {
        Entities.Project GetLastProject(int userId);
    }
}
