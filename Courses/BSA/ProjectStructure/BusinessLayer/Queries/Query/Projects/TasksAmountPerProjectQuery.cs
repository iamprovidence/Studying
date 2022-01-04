namespace BusinessLayer.Queries.Query.Projects
{
    public class TasksAmountPerProjectQuery : Interfaces.IQuery<System.Collections.Generic.IDictionary<string, int>>
    {
        public int UserId { get; private set; }

        public TasksAmountPerProjectQuery(int userId)
        {
            this.UserId = userId;
        }
    }
}
