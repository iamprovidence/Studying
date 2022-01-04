namespace BusinessLayer.Queries.Query.Projects
{
    public class SingleProjectQuery : BusinessLayer.Interfaces.IQuery<DataAccessLayer.Entities.Project>
    {
        public int Id { get; private set; }

        public SingleProjectQuery(int id)
        {
            this.Id = id;
        }
    }
}
