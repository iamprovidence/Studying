using Core.DataTransferObjects.User;

using DataAccessLayer.Entities;

namespace BusinessLayer.MappingProfiles
{
    class UserProfile : AutoMapper.Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserListDTO>();

            CreateMap<User, UserViewDTO>()
                .ForMember(dest => dest.TeamName, opt => opt.MapFrom(src => src.TeamId != null ? src.Team.Name : string.Empty));

            CreateMap<CreateUserDTO, User>()
                .AfterMap((s, d) => d.RegisteredAt = System.DateTime.Now);

            CreateMap<UpdateUserDTO, User>();

        }
    }
}
