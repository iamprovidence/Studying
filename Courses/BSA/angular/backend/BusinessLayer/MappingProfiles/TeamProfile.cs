using Core.DataTransferObjects.Team;

using DataAccessLayer.Entities;

namespace BusinessLayer.MappingProfiles
{
    public class TeamProfile : AutoMapper.Profile
    {
        public TeamProfile()
        {
            CreateMap<Team, TeamListDTO>();

            CreateMap<Team, TeamViewDTO>();

            CreateMap<CreateTeamDTO, Team>()
                .AfterMap((s, d) => d.CreatedAt = System.DateTime.Now);

            CreateMap<UpdateTeamDTO, Team>();

        }
    }
}
