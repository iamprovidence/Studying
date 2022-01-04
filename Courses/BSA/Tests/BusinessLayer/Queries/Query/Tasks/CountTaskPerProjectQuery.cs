namespace BusinessLayer.Queries.Query.Tasks
{
    public class CountTaskPerProjectQuery : Interfaces.IQuery<int>
    {
        public int UserId { get; private set; }
        public int ProjectId { get; private set; }

        public CountTaskPerProjectQuery(int userId, int projectId)
        {
            this.UserId = userId;
            this.ProjectId = projectId;
        }
    }
}
