using DataAccess.Models;
using DataAccess.Services;
using DataAccess.DataProviders;
using DataAccess.Models.DataTransferObjects;

using System;
using System.Collections.Generic;

namespace Linq
{
    class Menu
    {
        // FIELDS
        ProjectService projectService;
        TaskService taskService;
        UserService userService;

        bool exit;

        // CONSTRUCTORS
        public Menu()
        {
            projectService = new ProjectService(WebApiDataProvider.Instance);
            taskService = new TaskService(WebApiDataProvider.Instance);
            userService = new UserService(WebApiDataProvider.Instance);

        }

        // METHODS
        public void Run()
        {
            while (!exit)
            {
                ShowMenuItems();
                int userChoice = GetInput();
                Perform(userChoice);
            }
        }

        private void ShowMenuItems()
        {
            Console.Clear();

            Console.WriteLine("Choose option");

            Console.WriteLine();

            Console.WriteLine("0 - Get task amount per project by user ID");
            Console.WriteLine("1 - Get tasks with name <45 chars");
            Console.WriteLine("2 - Get finished task this year"); 
            Console.WriteLine("3 - Get team list with participants older than current value amount per project by user ID");
            Console.WriteLine("4 - Get ordered users and their tasks");
            Console.WriteLine("5 - Get user by ID");
            Console.WriteLine("6 - Get last user project");
            Console.WriteLine("7 - Get task amount on last user project");
            Console.WriteLine("8 - Get unfinished or closed task for user");
            Console.WriteLine("9 - Get the longest user's task");
            Console.WriteLine("10 - Get project by id");
            Console.WriteLine("11 - Get task with the longest description");
            Console.WriteLine("12 - Get task with the shortest name");
            Console.WriteLine("13 - Get filtered team participant amount");

            Console.WriteLine("14 - Exit");


            Console.WriteLine();
        }

        private int GetInput()
        {
            // get user choice
            int userChoice;
            do
            {
                Console.WriteLine("Your input:");
            } while (!int.TryParse(Console.ReadLine(), out userChoice));

            return userChoice;
        }

        private void Perform(int userChoice)
        {
            switch (userChoice)
            {
                case 0: GetTaskAmountPerProject(); break;
                case 1: GetShortTask(); break;
                case 2: GetFinishedThisYearTask(); break;
                case 3: GetTeamList(); break;
                case 4: GetOrderedUsers(); break;
                case 5: GetUserById(); break;
                case 6: GetLastUserProject(); break;
                case 7: GetTaskAmountInLastUserProject(); break;
                case 8: GetUnfinishedTask(); break;
                case 9: GetLongestTask(); break;
                case 10: GetProjectById(); break;
                case 11: GetTaskWithTheLongestDescription(); break;
                case 12: GetTaskWithTheShortestname(); break;
                case 13: GetFilteredTeamParticipantAmount(); break;

                case 14: Exit(); break;

                default: Console.WriteLine("Wrong choice"); break;
            }

            Console.WriteLine("Press enter");
            Console.ReadLine();
        }
                                    
        // ACTIONS
        private void GetTaskAmountPerProject()
        {
            // get input
            int userId;

            do
            {
                Console.WriteLine("Write User ID");
            } while (!int.TryParse(Console.ReadLine(), out userId));

            // show data
            foreach(KeyValuePair<Project, int> pair in projectService.GetTasksAmountPerProject(userId))
            {
                Console.WriteLine($"{pair.Key.Name} - {pair.Value}");
            }
        }

        private void GetShortTask()
        {
            // get input
            int userId;
            do
            {
                Console.WriteLine("Write User Id");
            } while (!int.TryParse(Console.ReadLine(), out userId));

            // show data
            foreach (Task task in taskService.GetUsersTasks(userId))
            {
                Console.WriteLine(task.Name);
            }
        }

        private void GetFinishedThisYearTask()
        {
            // get input
            int userId;
            do
            {
                Console.WriteLine("Write User Id");
            } while (!int.TryParse(Console.ReadLine(), out userId));

            // show data
            foreach (FinishedTaskDTO finishedTask in taskService.GetFinishedTasks(userId, finishedAtYear: DateTime.Now.Year))
            {
                Console.WriteLine($"{finishedTask.TaskId} - {finishedTask.TaskName}");
            }
        }

        private void GetTeamList()
        {
            // show data
            foreach (TeamUsersDTO team in userService.GetTeamByAgeLimit())
            {
                Console.WriteLine($"{team.TeamId} - {team.TeamName}");

                foreach (User participat in team.Participants)
                {
                    Console.WriteLine($"{participat.FirstName} {participat.RegisteredAt}".PadLeft(50));
                }
            }
        }

        private void GetOrderedUsers()
        {
            // show data
            foreach (UserTasksDTO userTasks in userService.GetOrderedUsersWithTasks())
            {
                Console.WriteLine($"{userTasks.User.FirstName}");

                foreach(Task task in userTasks.Tasks)
                {
                    Console.WriteLine($"\t{task.Name}");
                }
            }
        }

        private void GetUserById()
        {
            // get input
            int userId;
            do
            {
                Console.WriteLine("Write User Id");
            } while (!int.TryParse(Console.ReadLine(), out userId));

            // show data
            Console.WriteLine(userService.GetUser(userId));
        }

        private void GetLastUserProject()
        {
            // get input
            int userId;
            do
            {
                Console.WriteLine("Write User Id");
            } while (!int.TryParse(Console.ReadLine(), out userId));

            // show data
            Console.WriteLine(projectService.GetLastProject(userId)?.ToString());
        }

        private void GetTaskAmountInLastUserProject()
        {
            // get input
            int userId;
            do
            {
                Console.WriteLine("Write User Id");
            } while (!int.TryParse(Console.ReadLine(), out userId));

            int? lastProjectId = projectService.GetLastProject(userId)?.Id;

            // show data
            if (lastProjectId.HasValue) Console.WriteLine(taskService.CountTask(userId, lastProjectId.Value));
            else                        Console.WriteLine("User has no task in his last project");
        }
        
        private void GetUnfinishedTask()
        {
            // get input
            int userId;
            do
            {
                Console.WriteLine("Write User Id");
            } while (!int.TryParse(Console.ReadLine(), out userId));

            // show data
            Console.WriteLine($"Unfinished task amount - {taskService.CountExcept(userId)}");
        }
        private void GetLongestTask()
        {
            // get input
            int userId;
            do
            {
                Console.WriteLine("Write User Id");
            } while (!int.TryParse(Console.ReadLine(), out userId));

            // show data
            Console.WriteLine(taskService.GetLongestTask(userId));
        }
        private void GetProjectById()
        {
            // get input
            int projectId;
            do
            {
                Console.WriteLine("Write Project Id");
            } while (!int.TryParse(Console.ReadLine(), out projectId));

            // show data
            Console.WriteLine(projectService.GetProject(projectId));
        }

        private void GetFilteredTeamParticipantAmount()
        {
            foreach (KeyValuePair<Team, int> pair in userService.GetTeamUserAmount())
            {
                Console.WriteLine($"{pair.Key.Name} - {pair.Value}");
            }
        }

        private void GetTaskWithTheShortestname()
        {
            // get input
            int projectId;
            do
            {
                Console.WriteLine("Write Project Id");
            } while (!int.TryParse(Console.ReadLine(), out projectId));

            // show data
            Task task = taskService.GetTaskWithShortestName(projectId);
            if (task == null) Console.WriteLine("No task found");
            else              Console.WriteLine($"{task.Id} - {task.Name}");
        }

        private void GetTaskWithTheLongestDescription()
        {
            // get input
            int projectId;
            do
            {
                Console.WriteLine("Write Project Id");
            } while (!int.TryParse(Console.ReadLine(), out projectId));

            // show data
            Task task = taskService.GetTaskWithLongestDescription(projectId);
            if (task == null)  Console.WriteLine("No task found");
            else               Console.WriteLine($"{task.Id} - {task.Name} {task.Description}");
        }


        private void Exit()
        {
            exit = true;
        }
    }
}
