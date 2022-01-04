namespace Client.Interfaces
{
    public interface IServiceManager
    {
        ITaskService TaskService { get; }
        IProjectService ProjectService { get; }
        ITeamService TeamService { get; }
        IUserService UserService { get; }
        IMessageService MessageService { get; }
        IFileService FileService { get; }
    }
}
