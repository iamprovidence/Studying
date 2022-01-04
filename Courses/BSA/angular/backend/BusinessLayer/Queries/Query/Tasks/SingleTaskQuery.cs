namespace BusinessLayer.Queries.Query.Tasks
{
    public class SingleTaskQuery : Interfaces.IQuery<Core.DataTransferObjects.Task.TaskViewDTO>
    {
        public int Id { get; private set; }

        public SingleTaskQuery(int id)
        {
            this.Id = id;
        }
    }
}
