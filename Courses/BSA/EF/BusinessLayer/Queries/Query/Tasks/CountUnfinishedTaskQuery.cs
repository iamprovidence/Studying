namespace BusinessLayer.Queries.Query.Tasks
{
    public class CountUnfinishedTaskQuery : Interfaces.IQuery<int>
    {
        public int UserId { get; private set; }

        public CountUnfinishedTaskQuery(int userId)
        {
            this.UserId = userId;
        }
    }
}
