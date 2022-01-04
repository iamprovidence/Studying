using DataAccess.Models;
using DataAccess.Models.DataTransferObjects;

using System.Linq;
using System.Collections.Generic;

namespace DataAccess.Services
{
    public class TaskService
    {
        // FIELDS
        Interfaces.IDataProvider data;

        // CONSTRUCTORS
        public TaskService(Interfaces.IDataProvider dataProvider)
        {
            data = dataProvider;
        }

        // METHODS
        // Отримати список тасків, призначених для конкретного користувача (по id), де name таска <45 символів (колекція з тасків).
        public IEnumerable<Task> GetUsersTasks(int userId, int maxTaskNameLength = 45)
        {
            return data.Tasks
                        .Where(t => t.PerformerId == userId)
                        .Where(t => t.Name.Length < maxTaskNameLength);
        }

        // Отримати список (id, name) з колекції тасків, які виконані (finished) в 2019 році для конкретного користувача (по id).
        public IEnumerable<FinishedTaskDTO> GetFinishedTasks(int userId, int finishedAtYear = 2019)
        {
            return data.Tasks
                    .Where(t => t.FinishedAt.Year == finishedAtYear)
                    .Where(t => t.PerformerId == userId)
                    .Select(t => new FinishedTaskDTO
                    {
                        TaskId = t.Id,
                        TaskName = t.Name
                    });
        }

        // Отримати наступну структуру (передати Id користувача в параметри):
        // Загальна кількість тасків під останнім проектом

        // CountTask(userId, GetLastProject(userId).Id);
        public int CountTask(int userId, int projectId)
        {
            return data.Tasks
                       .Where(t => t.PerformerId == userId)
                       .Where(t => t.ProjectId == projectId)
                       .Count();
        }
        // Отримати наступну структуру(передати Id користувача в параметри) :
        // Загальна кількість незавершених або скасованих тасків для користувача
        public int CountExcept(int userId)
        {
            int finishedStatusId = data.TaskStates.First(s => s.Value == "Finished").Id;

            return CountExcept(userId, finishedStatusId);
        }
        public int CountExcept(int userId, int taskStatus)
        {
            return data.Tasks
                        .Where(t => t.PerformerId == userId)
                        .Where(t => t.StateId != taskStatus)
                        .Count();
        }
        // Отримати наступну структуру(передати Id користувача в параметри) :
        // Найтриваліший таск користувача за датою (найраніше створений - найпізніше закінчений)
        // P.S. - в даному випадку, статус таска не має значення, фільтруємо тільки за датою.
        public Task GetLongestTask(int userId)
        {
            return data.Tasks
                        .Where(t => t.PerformerId == userId)
                        .OrderByDescending(t => t.FinishedAt - t.CreatedAt)
                        .FirstOrDefault();
        }

        // Отримати таку структуру(передати Id проекту в параметри) :
        // Найдовший таск проекту(за описом)
        public Task GetTaskWithLongestDescription(int projectId)
        {
            return data.Tasks
                        .Where(t => t.ProjectId == projectId)
                        .OrderByDescending(t => t.Description.Length)
                        .FirstOrDefault();
        }

        // Отримати таку структуру(передати Id проекту в параметри) :
        // Найкоротший таск проекту(по імені)
        public Task GetTaskWithShortestName(int projectId)
        {
            return data.Tasks
                        .Where(t => t.ProjectId == projectId)
                        .OrderBy(t => t.Name.Length)
                        .FirstOrDefault();
        }
    }
}
