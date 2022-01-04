using BusinessLayer.Commands;

using DataAccessLayer.Entities;

using System.Collections.Generic;

namespace Client.Interfaces
{
    public interface IUserService
    {
        User Get(int id);
        IEnumerable<User> Get();
        IEnumerable<BusinessLayer.DataTransferObjects.UserTasksDTO> GetOrderedUsersWithTasks();

        CommandResponse Create(Core.DataTransferObjects.User.CreateUserDTO createUserDTO);

        CommandResponse Delete(int id);
    }
}
