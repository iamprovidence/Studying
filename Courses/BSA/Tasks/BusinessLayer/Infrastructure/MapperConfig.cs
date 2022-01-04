using AutoMapper;

namespace BusinessLayer.Infrastructure
{
    public class MapperConfig
    {
        public MapperConfiguration BuildConfiguration()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Core.DataTransferObjects.Task.CreateTaskDTO, DataAccessLayer.Entities.Task>();
                cfg.CreateMap<Core.DataTransferObjects.Team.CreateTeamDTO, DataAccessLayer.Entities.Team>();
                cfg.CreateMap<Core.DataTransferObjects.User.CreateUserDTO, DataAccessLayer.Entities.User>();
                cfg.CreateMap<Core.DataTransferObjects.Project.CreateProjectDTO, DataAccessLayer.Entities.Project>();
            });
        }   
    }
}
