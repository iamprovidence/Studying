namespace BusinessLayer.Queries.Query.Tasks
{
    public class ShortTaskQuery : Interfaces.IQuery<System.Collections.Generic.IEnumerable<DataAccessLayer.Entities.Task>>
    {
        public int UserId { get; private set; }
        public int MaxTaskNameLength { get; private set; }

        public ShortTaskQuery(int userId, int maxTaskNameLength = 45)
        {
            this.UserId = userId;
            this.MaxTaskNameLength = maxTaskNameLength;
        }
    }
}
