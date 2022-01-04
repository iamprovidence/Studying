using BusinessLayer.Interfaces;
using BusinessLayer.Queries.Query.Teams;
using BusinessLayer.DataTransferObjects;

using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;

using System.Linq;
using System.Threading.Tasks;
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
        public async Task<IEnumerable<TeamUsersDTO>> HandleAsync(TeamByAgeLimitQuery query)
        {
            ILookup<int?, User> teamUser = (await unitOfWork.GetRepository<User, UserRepository>().GetAsync()).ToLookup(k => k.TeamId);
            IEnumerable<Team> teamList = await unitOfWork.GetRepository<Team, TeamRepository>().GetAsync();

            return from t in teamList
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

        public async Task<IDictionary<int, int>> HandleAsync(UserInTeamAmountQuery query)
        {
            IEnumerable<Project> projectList = await unitOfWork.GetRepository<Project, ProjectRepository>().GetAsync();
            IEnumerable<DataAccessLayer.Entities.Task> taskList = await unitOfWork.GetRepository<DataAccessLayer.Entities.Task, TaskRepository>().GetAsync();

            IEnumerable<int> allowedProjectIds =
                (from k in
                    from p in projectList
                    where p.Description.Length > query.MinProjectDescriptionLength
                    select p.Id
                join l in
                    from t in taskList
                    group t by t.ProjectId into tp
                    where tp.Count() < query.MaxTaskAmount
                    select tp.Key
                on k equals l
                select k).ToList();

            IEnumerable<int?> allowedTeamId =
               (await unitOfWork.GetRepository<Project, ProjectRepository>().GetAsync(p => allowedProjectIds.Contains(p.Id)))
                    .Select(p => p.TeamId).ToList();

            ILookup<int?, User> teamUser = (await unitOfWork.GetRepository<User, UserRepository>().GetAsync()).ToLookup(u => u.TeamId);

            IEnumerable<int> teamsId = (await unitOfWork.GetRepository<Team, TeamRepository>().GetAsync(filter: t => allowedTeamId.Contains(t.Id))).Select(t => t.Id).ToList();

            return teamsId
                        .Select(x => new { TeamId = x, ParticipantAmount = teamUser[x].Count() })
                        .ToDictionary(k => k.TeamId, v => v.ParticipantAmount);            
        }

        public Task<IEnumerable<Team>> HandleAsync(AllTeamQuery query)
        {
            return unitOfWork.GetRepository<Team, TeamRepository>().GetAsync();
        }
    }
}
