namespace BusinessLayer.Queries.Query.Users
{
    public class SingleUserQuery : Interfaces.IQuery<DataAccessLayer.Entities.User>
    {
        public int UserId { get; private set; }

        public SingleUserQuery(int userId)
        {
            this.UserId = userId;
        }
    }
}
