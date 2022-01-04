using Core.DataTransferObjects.Project;

using DataAccessLayer.Entities;

namespace BusinessLayer.MappingProfiles
{
    public class ProjectProfile : AutoMapper.Profile
    {
        public ProjectProfile()
        {
            CreateMap<Project, ProjectViewDTO>()
                .ForMember(dest => dest.TeamName, opt => opt.MapFrom(src => src.TeamId != null ? src.Team.Name : string.Empty))
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => string.Join(" ", src.Author.FirstName, src.Author.LastName)));


            CreateMap<Project, ProjectListDTO>()
                .ForMember(dest => dest.ShortDescription, opt => opt.MapFrom(src => src.Description.Substring(0, System.Math.Min(15, src.Description.Length))));

            CreateMap<CreateProjectDTO, Project>()
                .AfterMap((s, d) => d.CreatedAt = System.DateTime.Now);

            CreateMap<UpdateProjectDTO, Project>();
        }
    }
}
