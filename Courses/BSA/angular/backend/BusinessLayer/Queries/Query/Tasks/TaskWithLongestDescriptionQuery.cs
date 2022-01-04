namespace BusinessLayer.Queries.Query.Tasks
{
    public class TaskWithLongestDescriptionQuery : Interfaces.IQuery<DataAccessLayer.Entities.Task>
    {
        public int ProjectId { get; private set; }

        public TaskWithLongestDescriptionQuery(int projectId)
        {
            this.ProjectId = projectId;
        }
    }
}
