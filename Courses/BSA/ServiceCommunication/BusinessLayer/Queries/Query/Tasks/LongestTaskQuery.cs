namespace BusinessLayer.Queries.Query.Tasks
{
    public class LongestTaskQuery : Interfaces.IQuery<DataAccessLayer.Entities.Task>
    {
        public int UserId { get; private set; }

        public LongestTaskQuery(int userId)
        {
            this.UserId = userId;
        }
    }
}
