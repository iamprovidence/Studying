using BusinessLayer.Commands;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Interfaces
{
    public interface ITeamService
    {
        Task<IEnumerable<DataAccessLayer.Entities.Team>> GetAsync();
        Task<IEnumerable<BusinessLayer.DataTransferObjects.TeamUsersDTO>> GetTeamByAgeLimitAsync(int participantsAge = 9);        
        Task<IDictionary<int, int>> GetTeamUserAmountAsync();

        Task<CommandResponse> CreateTeamAsync(Core.DataTransferObjects.Team.CreateTeamDTO createTeamDTO);

        Task<CommandResponse> RenameTeamAsync(Core.DataTransferObjects.Team.RenameTeamDTO value);

        Task<CommandResponse> DeleteTeamAsync(int id);

    }
}
