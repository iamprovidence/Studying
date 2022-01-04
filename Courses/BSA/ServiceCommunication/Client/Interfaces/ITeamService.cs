using System.Collections.Generic;

namespace Client.Interfaces
{
    public interface ITeamService
    {
        IEnumerable<DataAccessLayer.Entities.Team> Get();
        IEnumerable<BusinessLayer.DataTransferObjects.TeamUsersDTO> GetTeamByAgeLimit(int participantsAge = 9);        
        IDictionary<int, int> GetTeamUserAmount();
        
        bool CreateTeam(Core.DataTransferObjects.Team.CreateTeamDTO createTeamDTO);

        bool RenameTeam(Core.DataTransferObjects.Team.RenameTeamDTO value);

        bool DeleteTeam(int id);

    }
}
