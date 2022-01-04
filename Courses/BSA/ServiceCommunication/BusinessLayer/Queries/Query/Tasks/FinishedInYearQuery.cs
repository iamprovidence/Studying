namespace BusinessLayer.Queries.Query.Tasks
{
    public class FinishedInYearQuery : Interfaces.IQuery<System.Collections.Generic.IEnumerable<DataAccessLayer.Entities.Task>>
    {
        public int UserId { get; private set; }
        public int Year { get; private set; }

        public FinishedInYearQuery(int userId)
            : this(userId, System.DateTime.Now.Year) { }

        public FinishedInYearQuery(int userId, int year)
        {
            this.UserId = userId;
            this.Year = year;
        }
    }
}
