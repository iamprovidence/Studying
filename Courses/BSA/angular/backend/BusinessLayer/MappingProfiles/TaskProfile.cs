using Core.DataTransferObjects.Task;
using Core.DataTransferObjects.TaskState;

using DataAccessLayer.Entities;

namespace BusinessLayer.MappingProfiles
{
    public class TaskProfile : AutoMapper.Profile
    {
        public TaskProfile()
        {
            CreateMap<TaskState, TaskStateListDTO>();

            CreateMap<Task, TaskViewDTO>()
                .ForMember(dest => dest.PerformerName, 
                    opt => opt.MapFrom(src => src.PerformerId != null ? string.Join(" ", src.Performer.FirstName, src.Performer.LastName) : string.Empty))
                .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project.Name))
                .ForMember(dest => dest.StateName, opt => opt.MapFrom(src => src.TaskState.Value));

            CreateMap<Task, TaskListDTO>()
                .ForMember(dest => dest.ShortDescription, opt => opt.MapFrom(src => src.Description.Substring(0, System.Math.Min(15, src.Description.Length))));

            CreateMap<CreateTaskDTO, Task>()
                .AfterMap((s, d) => d.CreatedAt = System.DateTime.Now);            

            CreateMap<UpdateTaskDTO, Task>();
        }
    }
}
