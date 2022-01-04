namespace BusinessLayer.Queries.Query.Users
{
    public class SingleUserQuery : Interfaces.IQuery<Core.DataTransferObjects.User.UserViewDTO>
    {
        public int UserId { get; private set; }

        public SingleUserQuery(int userId)
        {
            this.UserId = userId;
        }
    }
}
