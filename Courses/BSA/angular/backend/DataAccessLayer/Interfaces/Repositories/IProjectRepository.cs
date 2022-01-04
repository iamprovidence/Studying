namespace DataAccessLayer.Interfaces.Repositories
{
    public interface IProjectRepository : IRepository<Entities.Project>
    {
        System.Threading.Tasks.Task<Entities.Project> GetLastProjectAsync(int userId);
    }
}
