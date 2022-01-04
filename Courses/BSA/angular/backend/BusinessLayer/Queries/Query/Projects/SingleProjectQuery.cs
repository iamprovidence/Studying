namespace BusinessLayer.Queries.Query.Projects
{
    public class SingleProjectQuery : Interfaces.IQuery<Core.DataTransferObjects.Project.ProjectViewDTO>
    {
        public int Id { get; private set; }

        public SingleProjectQuery(int id)
        {
            this.Id = id;
        }
    }
}
