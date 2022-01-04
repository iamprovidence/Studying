namespace DataAccessLayer.Repositories
{
    public class TeamRepository : RepositoryBase<Entities.Team>, Interfaces.Repositories.ITeamRepository
    {
        public TeamRepository()
            : base() { }
               

        public TeamRepository(Microsoft.EntityFrameworkCore.DbContext dbContext) 
            : base(dbContext) { }
    }
}
