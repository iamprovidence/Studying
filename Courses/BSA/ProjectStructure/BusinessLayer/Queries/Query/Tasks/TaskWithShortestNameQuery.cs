namespace BusinessLayer.Queries.Query.Tasks
{
    public class TaskWithShortestNameQuery : Interfaces.IQuery<DataAccessLayer.Entities.Task>
    {
        public int ProjectId { get; private set; }

        public TaskWithShortestNameQuery(int projectId)
        {
            this.ProjectId = projectId;
        }
    }
}
