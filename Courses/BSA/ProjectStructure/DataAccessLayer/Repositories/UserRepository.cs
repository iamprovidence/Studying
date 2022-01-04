using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public class UserRepository : RepositoryBase<Entities.User>, Interfaces.Repositories.IUserRepository
    {
        public UserRepository()
            : base() { }

        public UserRepository(DbContext dbContext) 
            : base(dbContext) { }
    }
}
