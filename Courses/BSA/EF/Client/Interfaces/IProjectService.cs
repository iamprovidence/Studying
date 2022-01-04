namespace Client.Interfaces
{
    public interface IProjectService
    {
        DataAccessLayer.Entities.Project GetProject(int id);
        System.Collections.Generic.IEnumerable<DataAccessLayer.Entities.Project> GetAll();
        System.Collections.Generic.IDictionary<string, int> GetTasksAmountPerProject(int userId);
        DataAccessLayer.Entities.Project GetLastProject(int userId);

        BusinessLayer.Commands.CommandResponse Create(Core.DataTransferObjects.Project.CreateProjectDTO createProjectDTO);

        BusinessLayer.Commands.CommandResponse Delete(int id);
    }
}
