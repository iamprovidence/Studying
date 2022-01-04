using System.Linq;

namespace BusinessLayer.DataTransferObjects
{
    public class UserTasksDTO : System.IComparable<UserTasksDTO>
    {
        public string UserName { get; set; }
        public System.Collections.Generic.IEnumerable<string> TaskNames { get; set; }

        public int CompareTo(UserTasksDTO other)
        {
            return this.UserName.CompareTo(other.UserName);
        }
    }
}
