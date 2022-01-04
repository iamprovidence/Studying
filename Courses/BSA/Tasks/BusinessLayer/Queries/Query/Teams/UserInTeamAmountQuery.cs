namespace BusinessLayer.Queries.Query.Teams
{
    public class UserInTeamAmountQuery : Interfaces.IQuery<System.Collections.Generic.IDictionary<int, int>>
    {
        public int MinProjectDescriptionLength { get; private set; }
        public int MaxTaskAmount { get; private set; }

        public UserInTeamAmountQuery()
            : this(25, 3) { }

        public UserInTeamAmountQuery(int minProjectDescriptionLength, int maxTaskAmount)
        {
            this.MinProjectDescriptionLength = minProjectDescriptionLength;
            this.MaxTaskAmount = maxTaskAmount;
        }
    }
}
