using BusinessLayer.Interfaces;
using BusinessLayer.Queries.Query.Teams;
using BusinessLayer.DataTransferObjects;

using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;

using System.Linq;
using System.Collections.Generic;

namespace BusinessLayer.Queries.Handler.Teams
{
    public class TeamQueryHandler : 
        IQueryHandler<TeamByAgeLimitQuery, IEnumerable<TeamUsersDTO>>,
        IQueryHandler<UserInTeamAmountQuery, IDictionary<int, int>>,
        IQueryHandler<AllTeamQuery, IEnumerable<Team>>,
        IUnitOfWorkSettable
    {
        // FIELDS
        IUnitOfWork unitOfWork;

        // CONSTRUCTORS
        public TeamQueryHandler()
        {
            unitOfWork = null;
        }
        public void SetUnitOfWork(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // METHODS
        public IEnumerable<TeamUsersDTO> Handle(TeamByAgeLimitQuery query)
        {
            ILookup<int?, User> teamUser = unitOfWork.GetRepository<User, UserRepository>().Get().ToLookup(k => k.TeamId);

            return from t in unitOfWork.GetRepository<Team, TeamRepository>().Get()
                   where teamUser.Contains(t.Id) && teamUser[t.Id].All(participant => OlderThan(participant, query.ParticipantAge))
                   select new TeamUsersDTO
                   {
                       TeamId = t.Id,
                       TeamName = t.Name,
                       Participants = teamUser[t.Id].OrderByDescending(u => u.RegisteredAt)
                   };
        }
        private bool OlderThan(User user, int allowedAge)
        {
            return System.DateTime.Now.Year - user.Birthday.Year >= allowedAge;
        }

        public IDictionary<int, int> Handle(UserInTeamAmountQuery query)
        {
                IEnumerable<int> allowedProjectIds =
                    from k in
                        from p in unitOfWork.GetRepository<Project, ProjectRepository>().Get()
                        where p.Description.Length > query.MinProjectDescriptionLength
                        select p.Id
                    join l in
                        from t in unitOfWork.GetRepository<Task, TaskRepository>().Get()
                        group t by t.ProjectId into tp
                        where tp.Count() < query.MaxTaskAmount
                        select tp.Key
                    on k equals l
                    select k;

                IEnumerable<int?> allowedTeamId =
                    unitOfWork.GetRepository<Project, ProjectRepository>().Get(p => allowedProjectIds.Contains(p.Id))
                        .Select(p => p.TeamId);

                ILookup<int?, User> teamUser = unitOfWork.GetRepository<User, UserRepository>().Get().ToLookup(u => u.TeamId);

                return unitOfWork.GetRepository<Team, TeamRepository>()
                            .Get(filter: t => allowedTeamId.Contains(t.Id))
                            .ToDictionary(k => k.Id, v => teamUser[v.Id].Count());            
        }

        public IEnumerable<Team> Handle(AllTeamQuery query)
        {
            return unitOfWork.GetRepository<Team, TeamRepository>().Get();
        }
    }
}
