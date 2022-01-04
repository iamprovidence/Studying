namespace DataAccessLayer.Interfaces.Repositories
{
    public interface ITaskRepository : IRepository<Entities.Task>
    {
        int Count(int userId, int projectId);
        int CountExcept(int userId);
        int CountExcept(int userId, int taskStatus);

        Entities.Task GetWithLongestDescription(int projectId);
        Entities.Task GetWithShortestName(int projectId);
        Entities.Task GetLongestTask(int userId);
    }
}
