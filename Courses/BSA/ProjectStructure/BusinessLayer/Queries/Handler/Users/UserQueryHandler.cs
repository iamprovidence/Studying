using BusinessLayer.Interfaces;
using BusinessLayer.Queries.Query.Users;
using BusinessLayer.DataTransferObjects;

using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;

using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BusinessLayer.Queries.Handler.Users
{
    public class UserQueryHandler : 
        IQueryHandler<SingleUserQuery, User>,
        IQueryHandler<OrderedUserQuery, IEnumerable<UserTasksDTO>>,
        IQueryHandler<AllUsersQuery, IEnumerable<User>>,
        IUnitOfWorkSettable
    {
        // FIELDS
        IUnitOfWork unitOfWork;

        // CONSTRUCTORS
        public UserQueryHandler()
        {
            unitOfWork = null;
        }        
        public void SetUnitOfWork(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // METHODS
        public Task<User> HandleAsync(SingleUserQuery query)
        {
            return unitOfWork.GetRepository<User, UserRepository>().GetAsync(query.UserId);
        }

        public async Task<IEnumerable<UserTasksDTO>> HandleAsync(OrderedUserQuery query)
        {
            if (query == null) throw new System.ArgumentNullException(nameof(query));

            IEnumerable<User> usersList = await unitOfWork.GetRepository<User, UserRepository>().GetAsync();
            ILookup<int?, DataAccessLayer.Entities.Task> performerTasks = (await unitOfWork.GetRepository<DataAccessLayer.Entities.Task, TaskRepository>().GetAsync()).ToLookup(t => t.PerformerId);

            return from u in usersList
                   orderby u.FirstName
                   select new UserTasksDTO
                   {
                       UserName = $"{u.FirstName} {u.LastName}",
                       TaskNames = performerTasks[u.Id].Select(t => t.Name)
                   };
        }

        public Task<IEnumerable<User>> HandleAsync(AllUsersQuery query)
        {
            return unitOfWork.GetRepository<User, UserRepository>().GetAsync();
        }
    }
}
