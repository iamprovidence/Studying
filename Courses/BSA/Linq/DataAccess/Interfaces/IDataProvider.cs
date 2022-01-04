using DataAccess.Models;

using System.Collections.Generic;

namespace DataAccess.Interfaces
{
    public interface IDataProvider
    {
        IEnumerable<User> Users { get; }
        IEnumerable<Task> Tasks { get; }
        IEnumerable<Project> Projects { get; }
        IEnumerable<Team> Teams { get; }
        IEnumerable<TaskState> TaskStates { get; }
    }
}
