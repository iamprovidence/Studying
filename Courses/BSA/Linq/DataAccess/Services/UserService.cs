using DataAccess.Models;
using DataAccess.Models.DataTransferObjects;

using System.Linq;
using System.Collections.Generic;

namespace DataAccess.Services
{
    public class UserService
    {
        // FIELDS
        Interfaces.IDataProvider data;

        // CONSTRUCTORS
        public UserService(Interfaces.IDataProvider dataProvider)
        {
            data = dataProvider;
        }

        // METHODS
        // Отримати список (id, ім'я команди і список користувачів) з колекції команд, 
        // учасники яких старші 12 років, відсортованих за датою реєстрації користувача за спаданням, 
        // а також згрупованих по командах. 
        // P.S - в цьому запиті допускається перевірка лише року народження користувача, без прив'язки до місяця/дня/часу народження.
        public IEnumerable<TeamUsersDTO> GetTeamByAgeLimit(int participantsAge = 9)
        {
            ILookup<int?, User> teamUser = data.Users.ToLookup(k => k.TeamId);

            return from t in data.Teams
                   where teamUser.Contains(t.Id) && teamUser[t.Id].All(participant => OlderThan(participant, participantsAge))
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

        // Отримати список користувачів за алфавітом first_name (по зростанню) з відсортованими tasks по довжині name (за спаданням).
        public IEnumerable<UserTasksDTO> GetOrderedUsersWithTasks()
        {
            return from u in data.Users
                   orderby u.FirstName
                   select new UserTasksDTO
                   {
                       User = u,
                       Tasks = data.Tasks
                                    .Where(t => t.PerformerId == u.Id)
                                    .OrderByDescending(t => t.Name.Length)
                   };
        }

        // Отримати наступну структуру(передати Id користувача в параметри)
        // User
        public User GetUser(int id)
        {
            return data.Users.FirstOrDefault(u => u.Id == id);
        }

        
        // Загальна кількість користувачів в команді проекту, де або опис проекту >25 символів, або кількість тасків <3
        public IDictionary<Team, int> GetTeamUserAmount(int minProjectDescriptionLength = 25, int maxTaskAmount = 3)
        {
            IEnumerable<int> allowedProjectIds =
                from k in
                    from p in data.Projects
                    where p.Description.Length > minProjectDescriptionLength
                    select p.Id
                join l in
                    from t in data.Tasks
                    group t by t.ProjectId into tp
                    where tp.Count() < maxTaskAmount
                    select tp.Key
                on k equals l
                select k;

            IEnumerable<int> allowedTeamId =
                data.Projects
                    .Where(p => allowedProjectIds.Contains(p.Id))
                    .Select(p => p.TeamId);

            ILookup<int?, User> teamUser = data.Users.ToLookup(u => u.TeamId);

            return data.Teams
                        .Where(t => allowedTeamId.Contains(t.Id))
                        .ToDictionary(k => k, v => teamUser[v.Id].Count());
        }
    }
}
