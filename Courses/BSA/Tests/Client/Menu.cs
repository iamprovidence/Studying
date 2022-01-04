using System;
using System.Timers;
using System.Threading.Tasks;
using System.Collections.Generic;

using DataAccessLayer.Entities;

using Unity;

using BusinessLayer.Commands;
using BusinessLayer.DataTransferObjects;

using Core.DataTransferObjects.User;
using Core.DataTransferObjects.Project;
using Core.DataTransferObjects.RabbitMQ;

using Client.EventArgs;

using Task = System.Threading.Tasks.Task;
using TaskDb = DataAccessLayer.Entities.Task;
using System.Collections;

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
        public async Task RunAsync()
        {
            while (!exit)
            {
                ShowMenuItems();
                int userChoice = GetInput();
                try
                {
                    await PerformAsync(userChoice);
                }
                catch (Exception ex)
                {
                    WriteLine(ex.Message, ConsoleColor.DarkYellow);
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

            Console.WriteLine("29 - Execute random query");
            Console.WriteLine();

            Console.WriteLine("30 - Exit");


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

        private async Task PerformAsync(int userChoice)
        {            
            switch (userChoice)
            {
                case 0: await GetAllTaskAsync(); break;
                case 1: await GetTaskByIdAsync(); break;
                case 2: await DeleteTaskByIdAsync(); break;
                case 3: await CreateRandomTaskAsync(); break;
                case 4: await GetTaskAmountPerProjectAsync(); break;                    
                case 5: await GetShortTaskAsync(); break;
                case 6: await GetFinishedThisYearTaskAsync(); break;
                case 7: await GetTaskAmountInLastUserProjectAsync(); break;
                case 8: await GetUnfinishedTaskAsync(); break;
                case 9: await GetLongestTaskAsync(); break;
                case 10: await GetTaskWithTheLongestDescriptionAsync(); break;
                case 11: await GetTaskWithTheShortestNameAsync(); break;

                case 12: await GetAllTeamAsync(); break;
                case 13: await GetFilteredTeamParticipantAmountAsync(); break;
                case 14: await GetTeamListAsync(); break;
                case 15: await CreateTeamAsync(); break;
                case 16: await RenameTeamAsync(); break;
                case 17: await DeleteTeamAsync(); break;

                case 18: await GetUserByIdAsync(); break;
                case 19: await GetAllUsersAsync(); break;
                case 20: await GetOrderedUsersAsync(); break;
                case 21: await CreateUserAsync(); break;
                case 22: await DeleteUserAsync(); break;

                case 23: await GetProjectByIdAsync(); break;
                case 24: await GetAllProjectsAsync(); break;
                case 25: await GetLastUserProjectAsync(); break;
                case 26: await CreateProjectAsync(); break;
                case 27: await DeleteProjectAsync(); break;

                case 28: await ActionCallInfoAsync(); break;

                case 29: ExecuteRandomQuery(); break;

                case 30: await ExitAsync(); break;

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
        private async Task GetAllTaskAsync()
        {
            // show data
            foreach (TaskDb task in await serviceManager.TaskService.GetAsync())
            {
                Console.WriteLine(task.ToString());
            }
        }
        private async Task GetTaskByIdAsync()
        {
            // input
            int taskId;
            do
            {
                Console.WriteLine("Write task id");
            } while (!int.TryParse(Console.ReadLine(), out taskId));

            // show data
            Console.WriteLine(await serviceManager.TaskService.GetAsync(taskId));            
        }
        private async Task DeleteTaskByIdAsync()
        {
            // input
            int taskId;
            do
            {
                Console.WriteLine("Write task id");
            } while (!int.TryParse(Console.ReadLine(), out taskId));

            // delete
            CommandResponse response = await serviceManager.TaskService.DeleteTaskAsync(taskId);
            WriteLine(response.Message, response.IsSucessed);
        }
        private async Task CreateRandomTaskAsync()
        {
            CommandResponse response = await serviceManager.TaskService.CreateTaskAsync(CrateRandomTaskDTO());
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

        private async Task GetTaskAmountPerProjectAsync()
        {
            // get input
            int userId;

            do
            {
                Console.WriteLine("Write User ID");
            } while (!int.TryParse(Console.ReadLine(), out userId));

            // show data
            foreach (KeyValuePair<string, int> pair in await serviceManager.ProjectService.GetTasksAmountPerProjectAsync(userId))
            {
                Console.WriteLine($"{pair.Key} - {pair.Value}");
            }
        }
        private async Task GetShortTaskAsync()
        {
            // get input
            int userId;
            do
            {
                Console.WriteLine("Write User Id");
            } while (!int.TryParse(Console.ReadLine(), out userId));

            // show data
            foreach (TaskDb task in await serviceManager.TaskService.GetTasksWithShortNameAsync(userId))
            {
                Console.WriteLine(task.Name);
            }
        }

        private async Task GetFinishedThisYearTaskAsync()
        {
            // get input
            int userId;
            do
            {
                Console.WriteLine("Write User Id");
            } while (!int.TryParse(Console.ReadLine(), out userId));

            // show data
            foreach (TaskDb task in await serviceManager.TaskService.GetFinishedTasksAsync(userId))
            {
                Console.WriteLine($"{task.Id} - {task.Name}");
            }
        }
        private async Task GetTaskAmountInLastUserProjectAsync()
        {
            // get input
            int userId;
            do
            {
                Console.WriteLine("Write User Id");
            } while (!int.TryParse(Console.ReadLine(), out userId));

            int? lastProjectId = (await serviceManager.ProjectService.GetLastProjectAsync(userId))?.Id;

            // show data
            if (lastProjectId.HasValue) Console.WriteLine(serviceManager.TaskService.CountTaskAsync(userId, lastProjectId.Value));
            else                        Console.WriteLine("User has no task in his last project");
        }
        private async Task GetUnfinishedTaskAsync()
        {
            // get input
            int userId;
            do
            {
                Console.WriteLine("Write User Id");
            } while (!int.TryParse(Console.ReadLine(), out userId));

            // show data
            Console.WriteLine($"Unfinished task amount - {await serviceManager.TaskService.UnfinishedTaskAmountAsync(userId)}");
        }
        private async Task GetLongestTaskAsync()
        {
            // get input
            int userId;
            do
            {
                Console.WriteLine("Write User Id");
            } while (!int.TryParse(Console.ReadLine(), out userId));

            // show data
            Console.WriteLine(await serviceManager.TaskService.GetLongestTaskAsync(userId));
        }
        private async Task GetTaskWithTheLongestDescriptionAsync()
        {
            // get input
            int projectId;
            do
            {
                Console.WriteLine("Write Project Id");
            } while (!int.TryParse(Console.ReadLine(), out projectId));

            // show data
            TaskDb task = await serviceManager.TaskService.GetTaskWithLongestDescriptionAsync(projectId);
            if (task == null)   Console.WriteLine("No task found");
            else                Console.WriteLine($"{task.Id} - {task.Name} {task.Description}");
        }
        private async Task GetTaskWithTheShortestNameAsync()
        {
            // get input
            int projectId;
            do
            {
                Console.WriteLine("Write Project Id");
            } while (!int.TryParse(Console.ReadLine(), out projectId));

            // show data
            TaskDb task = await serviceManager.TaskService.GetTaskWithShortestNameAsync(projectId);
            if (task == null)   Console.WriteLine("No task found");
            else                Console.WriteLine($"{task.Id} - {task.Name}");
        }
        #endregion

        #region TEAMS
        private async Task GetAllTeamAsync()
        {
            // show data
            foreach (Team team in await serviceManager.TeamService.GetAsync())
            {
                Console.WriteLine(team.ToString());
            }
        }
        private async Task GetFilteredTeamParticipantAmountAsync()
        {
            foreach (KeyValuePair<int, int> pair in await serviceManager.TeamService.GetTeamUserAmountAsync())
            {
                Console.WriteLine($"Team id = {pair.Key} - participant  amount = {pair.Value}");
            }
        }
        private async Task GetTeamListAsync()
        {
            // show data
            foreach (TeamUsersDTO team in await serviceManager.TeamService.GetTeamByAgeLimitAsync())
            {
                Console.WriteLine($"{team.TeamId} - {team.TeamName}");

                foreach (User participat in team.Participants)
                {
                    Console.WriteLine($"{participat.FirstName} {participat.RegisteredAt}".PadLeft(50));
                }
            }
        }
        private async Task CreateTeamAsync()
        {
            // create
            CommandResponse response = await serviceManager.TeamService.CreateTeamAsync(new Core.DataTransferObjects.Team.CreateTeamDTO
            {
                Name = RandomString(15),
                CreatedAt = DateTime.Now.AddMonths(-random.Next(5))
            });

            // show data
            WriteLine(response.Message, response.IsSucessed);
        }
        private async Task RenameTeamAsync()
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
            CommandResponse response = await serviceManager.TeamService.RenameTeamAsync(new Core.DataTransferObjects.Team.RenameTeamDTO
            {
                Id = teamId,
                Name = teamName
            });

            // show result
            WriteLine(response.Message, response.IsSucessed);
        }

        private async Task DeleteTeamAsync()
        {
            // get input
            int teamId;
            do
            {
                Console.WriteLine("Write team id");
            } while (!int.TryParse(Console.ReadLine(), out teamId));

            // delete
            CommandResponse result = await serviceManager.TeamService.DeleteTeamAsync(teamId);

            // show result
            WriteLine(result.Message, result.IsSucessed);
        }
        #endregion

        #region USERS
        private async Task GetUserByIdAsync()
        {
            // get input
            int userId;
            do
            {
                Console.WriteLine("Write User Id");
            } while (!int.TryParse(Console.ReadLine(), out userId));

            // show data
            Console.WriteLine(await serviceManager.UserService.GetAsync(userId));
        }
        private async Task GetAllUsersAsync()
        {
            // show data
            foreach (User user in await serviceManager.UserService.GetAsync())
            {
                Console.WriteLine(user.ToString());
            }
        }
        private async Task GetOrderedUsersAsync()
        {
            // show data
            foreach (UserTasksDTO userTasks in await serviceManager.UserService.GetOrderedUsersWithTasksAsync())
            {
                Console.WriteLine($"{userTasks.UserName}");

                foreach (string task in userTasks.TaskNames)
                {
                    Console.WriteLine($"\t{task}");
                }
            }
        }

        private async Task CreateUserAsync()
        {
            // create
            CommandResponse response = await serviceManager.UserService.CreateAsync(CreateRandomUser());

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

        private async Task DeleteUserAsync()
        {
            // get input
            int userId;
            do
            {
                Console.WriteLine("Write user id");
            } while (!int.TryParse(Console.ReadLine(), out userId));

            // delete
            CommandResponse response = await serviceManager.UserService.DeleteAsync(userId);

            // show result
            WriteLine(response.Message, response.IsSucessed);
        }
        #endregion

        #region PROJECTS
        private async Task GetProjectByIdAsync()
        {
            // get input
            int projectId;
            do
            {
                Console.WriteLine("Write Project Id");
            } while (!int.TryParse(Console.ReadLine(), out projectId));

            // show data
            Console.WriteLine(await serviceManager.ProjectService.GetProjectAsync(projectId));
        }
        private async Task GetAllProjectsAsync()
        {
            // show data
            foreach (Project project in await serviceManager.ProjectService.GetAllAsync())
            {
                Console.WriteLine(project.ToString());
            }
        }
        private async Task GetLastUserProjectAsync()
        {
            // get input
            int userId;
            do
            {
                Console.WriteLine("Write User Id");
            } while (!int.TryParse(Console.ReadLine(), out userId));

            // show data
            Console.WriteLine((await serviceManager.ProjectService.GetLastProjectAsync(userId))?.ToString());
        }
        
        private async Task CreateProjectAsync()
        {
            // creating
            CommandResponse response = await serviceManager.ProjectService.CreateAsync(CreateRandomProject());

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

        private async Task DeleteProjectAsync()
        {
            // get input
            int projectId;
            do
            {
                Console.WriteLine("Write project Id");
            } while (!int.TryParse(Console.ReadLine(), out projectId));

            // delete
            CommandResponse response = await serviceManager.ProjectService.DeleteAsync(projectId);

            // show result
            WriteLine(response.Message, response.IsSucessed);
        }
        #endregion


        private async Task ActionCallInfoAsync()
        {
            // get data
            IEnumerable<WorkerData> enumerable = await serviceManager.FileService.ActionCallListAsync();
            if (enumerable == null) return;

            // show data
            foreach (WorkerData data in enumerable)
            {
                Console.WriteLine(data.ToString());
            }
        }

        private async void ExecuteRandomQuery()
        {
            try
            {
                (int Index, string ClassName, string MethodName) queryInfo = await MarkRandomTaskWithDelay(GetQueries());

                WriteLine($"Executed query number '{queryInfo.Index}'. From '{queryInfo.ClassName}' called '{queryInfo.MethodName}'", true);
            }
            catch (Exception)
            {
                WriteLine("Failed to execute random task", false);
            }
        }
        private Func<object>[] GetQueries()
        {
            return new Func<object>[]
            {
                serviceManager.TaskService.GetAsync,                
                serviceManager.ProjectService.GetAllAsync,
                serviceManager.TeamService.GetAsync,
                serviceManager.UserService.GetAsync,
                serviceManager.FileService.ActionCallListAsync
            };
        }
        private Task<(int, string, string)> MarkRandomTaskWithDelay(Func<object>[] queries, int delay = 2000)
        {
            // defered execution
            // (int, string, string) — Index, ClassName, MethodName
            TaskCompletionSource<(int, string, string)> tsc = new TaskCompletionSource<(int, string, string)>();

            Timer timer = new Timer(delay);

            ElapsedEventHandler handler = null;
            handler = async (sender, eventArgs) =>
            {
                try
                {
                    // remove handler, so we do not change our task on next tick
                    timer.Elapsed -= handler;

                    int randomQueryIndex = random.Next(queries.Length);
                    // await, because we do not want to SetResult too early
                    await (queries[randomQueryIndex].Invoke() as Task);
                    
                    tsc.SetResult(
                        (randomQueryIndex, 
                        queries[randomQueryIndex].Method.DeclaringType.Name, 
                        queries[randomQueryIndex].Method.Name));
                }
                catch (Exception ex)
                {
                    tsc.SetException(ex);
                }
                finally
                {
                    timer.Stop();
                    timer.Dispose();
                }

            };
            timer.Elapsed += handler;
            timer.Start();

            return tsc.Task;
        }


        private void ReceiveMessageFromServer(object sender, SerealizationEventArgs e)
        {
            WriteLine(e.SerializationResult.Message, e.SerializationResult.IsSuccessed);
        }
        private async Task ExitAsync()
        {
            exit = true;
            await Task.CompletedTask;
        }
    }
}
