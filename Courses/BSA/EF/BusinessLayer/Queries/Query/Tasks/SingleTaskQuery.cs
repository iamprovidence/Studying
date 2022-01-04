namespace BusinessLayer.Queries.Query.Tasks
{
    public class SingleTaskQuery : Interfaces.IQuery<DataAccessLayer.Entities.Task>
    {
        public int Id { get; private set; }

        public SingleTaskQuery(int id)
        {
            this.Id = id;
        }
    }
}
