using DataAccessLayer.Interfaces;

namespace DataAccessLayer.Repositories
{
    public class TeamRepository : RepositoryBase<Entities.Team>, Interfaces.Repositories.ITeamRepository
    {
        public override void SetDataProvider(IDataProvider dataProvider)
        {
            entities = new System.Collections.Generic.List<Entities.Team>(dataProvider.Teams);
        }
    }
}
