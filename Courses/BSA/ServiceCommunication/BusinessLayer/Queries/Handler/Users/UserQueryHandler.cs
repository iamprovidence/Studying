using BusinessLayer.Interfaces;
using BusinessLayer.Queries.Query.Users;
using BusinessLayer.DataTransferObjects;

using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;

using System.Linq;
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
        public User Handle(SingleUserQuery query)
        {
            return unitOfWork.GetRepository<User, UserRepository>().Get(query.UserId);
        }

        public IEnumerable<UserTasksDTO> Handle(OrderedUserQuery query)
        {
            return from u in unitOfWork.GetRepository<User, UserRepository>().Get()
                   orderby u.FirstName
                   select new UserTasksDTO
                   {
                       User = u,
                       Tasks = unitOfWork.GetRepository<Task, TaskRepository>()
                                    .Get(
                                        filter: t => t.PerformerId == u.Id, 
                                        orderBy: q => q.OrderByDescending(t => t.Name.Length))
                   };
        }

        public IEnumerable<User> Handle(AllUsersQuery query)
        {
            return unitOfWork.GetRepository<User, UserRepository>().Get();
        }
    }
}
