using BusinessLayer.Commands;

using DataAccessLayer.Entities;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Interfaces
{
    public interface IUserService
    {
        Task<User> GetAsync(int id);
        Task<IEnumerable<User>> GetAsync();
        Task<IEnumerable<BusinessLayer.DataTransferObjects.UserTasksDTO>> GetOrderedUsersWithTasksAsync();

        Task<CommandResponse> CreateAsync(Core.DataTransferObjects.User.CreateUserDTO createUserDTO);

        Task<CommandResponse> DeleteAsync(int id);
    }
}
