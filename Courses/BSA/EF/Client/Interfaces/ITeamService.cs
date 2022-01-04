using BusinessLayer.Commands;

using System.Collections.Generic;

namespace Client.Interfaces
{
    public interface ITeamService
    {
        IEnumerable<DataAccessLayer.Entities.Team> Get();
        IEnumerable<BusinessLayer.DataTransferObjects.TeamUsersDTO> GetTeamByAgeLimit(int participantsAge = 9);        
        IDictionary<int, int> GetTeamUserAmount();

        CommandResponse CreateTeam(Core.DataTransferObjects.Team.CreateTeamDTO createTeamDTO);

        CommandResponse RenameTeam(Core.DataTransferObjects.Team.RenameTeamDTO value);

        CommandResponse DeleteTeam(int id);

    }
}
