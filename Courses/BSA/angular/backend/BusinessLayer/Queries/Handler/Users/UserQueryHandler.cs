using Core.DataTransferObjects.User;

using BusinessLayer.Interfaces;
using BusinessLayer.Queries.Query.Users;
using BusinessLayer.DataTransferObjects;

using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;

using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using AutoMapper;

namespace BusinessLayer.Queries.Handler.Users
{
    public class UserQueryHandler : 
        IQueryHandler<SingleUserQuery, UserViewDTO>,
        IQueryHandler<OrderedUserQuery, IEnumerable<UserTasksDTO>>,
        IQueryHandler<AllUsersQuery, IEnumerable<UserListDTO>>,
        IUnitOfWorkSettable, IMapperSettable
    {
        // FIELDS
        IMapper mapper;
        IUnitOfWork unitOfWork;

        // CONSTRUCTORS
        public UserQueryHandler()
        {
            mapper = null;
            unitOfWork = null;
        }        
        public void SetUnitOfWork(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public void SetMapper(IMapper mapper)
        {
            this.mapper = mapper;
        }

        // METHODS
        public async Task<UserViewDTO> HandleAsync(SingleUserQuery query)
        {
            User user = await unitOfWork.GetRepository<User, UserRepository>().GetAsync(query.UserId, includeProperties: nameof(User.Team));

            return mapper.Map<UserViewDTO>(user);
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

        public async Task<IEnumerable<UserListDTO>> HandleAsync(AllUsersQuery query)
        {
            IEnumerable<User> users = await unitOfWork.GetRepository<User, UserRepository>().GetAsync();            

            return mapper.Map<UserListDTO[]>(users);
        }

  }
}
