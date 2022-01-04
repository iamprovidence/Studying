using DataAccess.Models;

using System.Linq;
using System.Collections.Generic;

namespace DataAccess.Services
{
    public class ProjectService
    {
        // FIELDS
        Interfaces.IDataProvider data;

        // CONSTRUCTORS
        public ProjectService(Interfaces.IDataProvider dataProvider)
        {
            data = dataProvider;
        }

        // METHODS

        // Отримати кількість тасків у проекті конкретного користувача (по id) (словник, де key буде проект, а value кількість тасків).
        public IDictionary<Project, int> GetTasksAmountPerProject(int userId)
        {
            return (from t in data.Tasks
                    group t by t.ProjectId into tp
                    join p in data.Projects on tp.Key equals p.Id
                    where p.AuthorId == userId
                    select new
                    {
                        Project = p,
                        TaskAmount = tp.Count()
                    })
                   .ToDictionary(k => k.Project, v => v.TaskAmount);
        }

        // Отримати наступну структуру(передати Id користувача в параметри) :
        // Останній проект користувача(за датою створення)
        public Project GetLastProject(int userId)
        {
            return data.Projects
                        .Where(p => p.AuthorId == userId)
                        .OrderByDescending(p => p.CreatedAt)
                        .FirstOrDefault();
        }

        // Отримати таку структуру(передати Id проекту в параметри) :
        // Проект
        public Project GetProject(int id)
        {
            return data.Projects.FirstOrDefault(p => p.Id == id);
        }
    }
}
