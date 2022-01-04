namespace BusinessLayer.Queries.Query.Projects
{
    public class LastProjectQuery : Interfaces.IQuery<DataAccessLayer.Entities.Project>
    {
        public int UserId { get; private set; }

        public LastProjectQuery(int userId)
        {
            this.UserId = userId;
        }
    }
}
