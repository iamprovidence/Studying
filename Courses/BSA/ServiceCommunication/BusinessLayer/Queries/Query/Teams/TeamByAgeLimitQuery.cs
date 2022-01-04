using BusinessLayer.DataTransferObjects;

using System.Collections.Generic;

namespace BusinessLayer.Queries.Query.Teams
{
    public class TeamByAgeLimitQuery : Interfaces.IQuery<IEnumerable<TeamUsersDTO>>
    {
        public int ParticipantAge { get; private set; }

        public TeamByAgeLimitQuery(int participantAge)
        {
            this.ParticipantAge = participantAge;
        }
    }
}
