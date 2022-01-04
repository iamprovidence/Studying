using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;

namespace DataAccessLayer.Repositories
{
    public class UserRepository : RepositoryBase<User>, Interfaces.Repositories.IUserRepository
    {
        public override void SetDataProvider(IDataProvider dataProvider)
        {
            entities = new System.Collections.Generic.List<User>(dataProvider.Users);
        }
    }
}
