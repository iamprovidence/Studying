using System;
using System.Collections.Generic;

using DataAccessLayer.Entities;

using Unity;

using BusinessLayer.Commands;
using BusinessLayer.DataTransferObjects;

using Core.DataTransferObjects.User;
using Core.DataTransferObjects.Project;
using Core.DataTransferObjects.RabbitMQ;

using Client.EventArgs;

namespace Client
{
    class Menu : IDisposable
    {
        // FIELDS
        Interfaces.IServiceManager serviceManager;

        Random random;

        bool exit;

        // CONSTRUCTORS
        public Menu()
        {
            serviceManager = ServicesConfiguration.Container.Resolve<Interfaces.IServiceManager>();

            serviceManager.MessageService.Received += ReceiveMessageFromServer;

            random = new Random();
        }

        public void Dispose()
        {
            (serviceManager as IDisposable)?.Dispose();
        }


        // METHODS
        private void WriteLine(string text, ConsoleColor consoleColor)
        {
            Console.ForegroundColor = consoleColor;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        private void WriteLine(string text, bool result)
        {
            WriteLine(text, result ? ConsoleColor.Green : ConsoleColor.Red);
        }
        public void Run()
        {
            while (!exit)
            {
                ShowMenuItems();
                int userChoice = GetInput();
                try
                {
                    Perform(userChoice);
                }
                catch (AggregateException ex)
                {
                    WriteLine(ex.Message, ConsoleColor.DarkYellow);
                    WriteLine("Server may not be running", ConsoleColor.DarkYellow);
                    Console.ReadLine();
                }
            }
        }

        private void ShowMenuItems()
        {
            Console.Clear();

            Console.WriteLine("Choose option");

            Console.WriteLine();

            Console.WriteLine("0 - Get all task");
            Console.WriteLine("1 - Get task by id");
            Console.WriteLine("2 - Delete task by id");
            Console.WriteLine("3 - Create random task");
            Console.WriteLine("4 - Get task amount per project by user ID");
            Console.WriteLine("5 - Get tasks with name <45 chars");
            Console.WriteLine("6 - Get finished task this year");
            Console.WriteLine("7 - Get task amount on last user project");
            Console.WriteLine("8 - Get unfinished or closed task for user");
            Console.WriteLine("9 - Get the longest user's task");
            Console.WriteLine("10 - Get task with the longest description");
            Console.WriteLine("11 - Get task with the shortest name");
            Console.WriteLine();

            Console.WriteLine("12 - Get all teams");
            Console.WriteLine("13 - Get filtered team participant amount");
            Console.WriteLine("14 - Get team list with participants older than current value amount per project by user ID");
            Console.WriteLine("15 - Create random team");
            Console.WriteLine("16 - Rename team");
            Console.WriteLine("17 - Delete team");
            Console.WriteLine();

            Console.WriteLine("18 - Get user by ID");
            Console.WriteLine("19 - Get all users");
            Console.WriteLine("20 - Get ordered users and their tasks");
            Console.WriteLine("21 - Create random user");
            Console.WriteLine("22 - Delete user by ID");
            Console.WriteLine();

            Console.WriteLine("23 - Get project by id");
            Console.WriteLine("24 - Get all projects");
            Console.WriteLine("25 - Get last user project");
            Console.WriteLine("26 - Create random project");
            Console.WriteLine("27 - Delete project by ID");
            Console.WriteLine();

            Console.WriteLine("28 - Get all actions call info");
            Console.WriteLine();

            Console.WriteLine("29 - Exit");


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
                case 0: GetAllTask(); break;
                case 1: GetTaskById(); break;
                case 2: DeleteTaskById(); break;
                case 3: CreateRandomTask(); break;
                case 4: GetTaskAmountPerProject(); break;                    
                case 5: GetShortTask(); break;
                case 6: GetFinishedThisYearTask(); break;
                case 7: GetTaskAmountInLastUserProject(); break;
                case 8: GetUnfinishedTask(); break;
                case 9: GetLongestTask(); break;
                case 10: GetTaskWithTheLongestDescription(); break;
                case 11: GetTaskWithTheShortestName(); break;

                case 12: GetAllTeam(); break;
                case 13: GetFilteredTeamParticipantAmount(); break;
                case 14: GetTeamList(); break;
                case 15: CreateTeam(); break;
                case 16: RenameTeam(); break;
                case 17: DeleteTeam(); break;

                case 18: GetUserById(); break;
                case 19: GetAllUsers(); break;
                case 20: GetOrderedUsers(); break;
                case 21: CreateUser(); break;
                case 22: DeleteUser(); break;

                case 23: GetProjectById(); break;
                case 24: GetAllProjects(); break;
                case 25: GetLastUserProject(); break;
                case 26: CreateProject(); break;
                case 27: DeleteProject(); break;

                case 28: ActionCallInfo(); break;

                case 29: Exit(); break;

                default: Console.WriteLine("Wrong choice"); break;
            }

            Console.WriteLine("Press enter");
            Console.ReadLine();
        }


        public string RandomString(int length)
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder(length);

            for (int i = 0; i < length; ++i)
            {
                stringBuilder.Append((char)random.Next(97, 127));
            }
            return stringBuilder.ToString();
        }

        // ACTIONS
        #region TASKS
        private void GetAllTask()
        {
            // show data
            foreach (Task task in serviceManager.TaskService.Get())
            {
                Console.WriteLine(task.ToString());
            }
        }
        private void GetTaskById()
        {
            // input
            int taskId;
            do
            {
                Console.WriteLine("Write task id");
            } while (!int.TryParse(Console.ReadLine(), out taskId));

            // show data
            Console.WriteLine(serviceManager.TaskService.Get(taskId));
        }
        private void DeleteTaskById()
        {
            // input
            int taskId;
            do
            {
                Console.WriteLine("Write task id");
            } while (!int.TryParse(Console.ReadLine(), out taskId));

            // delete
            CommandResponse response = serviceManager.TaskService.DeleteTask(taskId);
            WriteLine(response.Message, response.IsSucessed);
        }
        private void CreateRandomTask()
        {
            CommandResponse response = serviceManager.TaskService.CreateTask(CrateRandomTaskDTO());
            WriteLine(response.Message, response.IsSucessed);
        }
        public Core.DataTransferObjects.Task.CreateTaskDTO CrateRandomTaskDTO()
        {
            return new Core.DataTransferObjects.Task.CreateTaskDTO
            {
                Name = RandomString(10),
                Description = RandomString(25),

                CreatedAt = DateTime.Now.Date,
                FinishedAt = DateTime.Now.AddYears(1).Date,

                PerformerId = random.Next(10),
                ProjectId = random.Next(10),
            };
        }

        private void GetTaskAmountPerProject()
        {
            // get input
            int userId;

            do
            {
                Console.WriteLine("Write User ID");
            } while (!int.TryParse(Console.ReadLine(), out userId));

            // show data
            foreach (KeyValuePair<string, int> pair in serviceManager.ProjectService.GetTasksAmountPerProject(userId))
            {
                Console.WriteLine($"{pair.Key} - {pair.Value}");
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
            foreach (Task task in serviceManager.TaskService.GetTasksWithShortName(userId))
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
            foreach (Task task in serviceManager.TaskService.GetFinishedTasks(userId))
            {
                Console.WriteLine($"{task.Id} - {task.Name}");
            }
        }
        private void GetTaskAmountInLastUserProject()
        {
            // get input
            int userId;
            do
            {
                Console.WriteLine("Write User Id");
            } while (!int.TryParse(Console.ReadLine(), out userId));

            int? lastProjectId = serviceManager.ProjectService.GetLastProject(userId)?.Id;

            // show data
            if (lastProjectId.HasValue) Console.WriteLine(serviceManager.TaskService.CountTask(userId, lastProjectId.Value));
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
            Console.WriteLine($"Unfinished task amount - {serviceManager.TaskService.UnfinishedTaskAmount(userId)}");
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
            Console.WriteLine(serviceManager.TaskService.GetLongestTask(userId));
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
            Task task = serviceManager.TaskService.GetTaskWithLongestDescription(projectId);
            if (task == null)   Console.WriteLine("No task found");
            else                Console.WriteLine($"{task.Id} - {task.Name} {task.Description}");
        }
        private void GetTaskWithTheShortestName()
        {
            // get input
            int projectId;
            do
            {
                Console.WriteLine("Write Project Id");
            } while (!int.TryParse(Console.ReadLine(), out projectId));

            // show data
            Task task = serviceManager.TaskService.GetTaskWithShortestName(projectId);
            if (task == null)   Console.WriteLine("No task found");
            else                Console.WriteLine($"{task.Id} - {task.Name}");
        }
        #endregion

        #region TEAMS
        private void GetAllTeam()
        {
            // show data
            foreach (Team team in serviceManager.TeamService.Get())
            {
                Console.WriteLine(team.ToString());
            }
        }
        private void GetFilteredTeamParticipantAmount()
        {
            foreach (KeyValuePair<int, int> pair in serviceManager.TeamService.GetTeamUserAmount())
            {
                Console.WriteLine($"Team id = {pair.Key} - participant  amount = {pair.Value}");
            }
        }
        private void GetTeamList()
        {
            // show data
            foreach (TeamUsersDTO team in serviceManager.TeamService.GetTeamByAgeLimit())
            {
                Console.WriteLine($"{team.TeamId} - {team.TeamName}");

                foreach (User participat in team.Participants)
                {
                    Console.WriteLine($"{participat.FirstName} {participat.RegisteredAt}".PadLeft(50));
                }
            }
        }
        private void CreateTeam()
        {
            // create
            CommandResponse response = serviceManager.TeamService.CreateTeam(new Core.DataTransferObjects.Team.CreateTeamDTO
            {
                Name = RandomString(15),
                CreatedAt = DateTime.Now.AddMonths(-random.Next(5))
            });

            // show data
            WriteLine(response.Message, response.IsSucessed);
        }
        private void RenameTeam()
        {
            // get input
            int teamId;
            do
            {
                Console.WriteLine("Write team id");
            } while (!int.TryParse(Console.ReadLine(), out teamId));            

            Console.WriteLine("Write team name");
            string teamName = Console.ReadLine();

            // rename
            CommandResponse response = serviceManager.TeamService.RenameTeam(new Core.DataTransferObjects.Team.RenameTeamDTO
            {
                Id = teamId,
                Name = teamName
            });

            // show result
            WriteLine(response.Message, response.IsSucessed);
        }

        private void DeleteTeam()
        {
            // get input
            int teamId;
            do
            {
                Console.WriteLine("Write team id");
            } while (!int.TryParse(Console.ReadLine(), out teamId));

            // delete
            CommandResponse result = serviceManager.TeamService.DeleteTeam(teamId);

            // show result
            WriteLine(result.Message, result.IsSucessed);
        }
        #endregion

        #region USERS
        private void GetUserById()
        {
            // get input
            int userId;
            do
            {
                Console.WriteLine("Write User Id");
            } while (!int.TryParse(Console.ReadLine(), out userId));

            // show data
            Console.WriteLine(serviceManager.UserService.Get(userId));
        }
        private void GetAllUsers()
        {
            // show data
            foreach (User user in serviceManager.UserService.Get())
            {
                Console.WriteLine(user.ToString());
            }
        }
        private void GetOrderedUsers()
        {
            // show data
            foreach (UserTasksDTO userTasks in serviceManager.UserService.GetOrderedUsersWithTasks())
            {
                Console.WriteLine($"{userTasks.UserName}");

                foreach (string task in userTasks.TaskNames)
                {
                    Console.WriteLine($"\t{task}");
                }
            }
        }

        private void CreateUser()
        {
            // create
            CommandResponse response = serviceManager.UserService.Create(CreateRandomUser());

            // show
            WriteLine(response.Message, response.IsSucessed);
        }
        private CreateUserDTO CreateRandomUser()
        {
            return new CreateUserDTO
            {
                FirstName = RandomString(10),
                LastName = RandomString(10),
                Email = RandomString(10),

                Birthday = DateTime.Now.AddMonths(-random.Next(5)),
                RegisteredAt = DateTime.Now.AddMonths(random.Next(5)),

                TeamId = random.Next(10)
            };
        }

        private void DeleteUser()
        {
            // get input
            int userId;
            do
            {
                Console.WriteLine("Write user id");
            } while (!int.TryParse(Console.ReadLine(), out userId));

            // delete
            CommandResponse response = serviceManager.UserService.Delete(userId);

            // show result
            WriteLine(response.Message, response.IsSucessed);
        }
        #endregion

        #region PROJECTS
        private void GetProjectById()
        {
            // get input
            int projectId;
            do
            {
                Console.WriteLine("Write Project Id");
            } while (!int.TryParse(Console.ReadLine(), out projectId));

            // show data
            Console.WriteLine(serviceManager.ProjectService.GetProject(projectId));
        }
        private void GetAllProjects()
        {
            // show data
            foreach (Project project in serviceManager.ProjectService.GetAll())
            {
                Console.WriteLine(project.ToString());
            }
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
            Console.WriteLine(serviceManager.ProjectService.GetLastProject(userId)?.ToString());
        }
        
        private void CreateProject()
        {
            // creating
            CommandResponse response = serviceManager.ProjectService.Create(CreateRandomProject());

            // show result
            WriteLine(response.Message, response.IsSucessed);
        }

        private CreateProjectDTO CreateRandomProject()
        {
            return new CreateProjectDTO
            {
                Name = RandomString(10),
                Description = RandomString(50),

                CreatedAt = DateTime.Now.AddMonths(-random.Next(5)),
                Deadline = DateTime.Now.AddMonths(random.Next(5)),

                AuthorId = random.Next(5),
                TeamId = random.Next(5)
            };
        }

        private void DeleteProject()
        {
            // get input
            int projectId;
            do
            {
                Console.WriteLine("Write project Id");
            } while (!int.TryParse(Console.ReadLine(), out projectId));

            // delete
            CommandResponse response = serviceManager.ProjectService.Delete(projectId);

            // show result
            WriteLine(response.Message, response.IsSucessed);
        }
        #endregion


        private void ActionCallInfo()
        {
            // get data
            IEnumerable<WorkerData> enumerable = serviceManager.FileService.ActionCallList();
            if (enumerable == null) return;

            // show data
            foreach (WorkerData data in enumerable)
            {
                Console.WriteLine($"[{data.Date.ToShortDateString()}] {data.ControllerActionName.ControllerName} - {data.ControllerActionName.ActionName}");
            }
        }
        private void ReceiveMessageFromServer(object sender, SerealizationEventArgs e)
        {
            WriteLine(e.SerializationResult.Message, e.SerializationResult.IsSuccessed);
        }

        private void Exit()
        {
            exit = true;
        }
    }
}
