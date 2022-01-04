using Xunit;

using BusinessLayer.Commands;

using Core.DataTransferObjects.Project;

using Microsoft.EntityFrameworkCore;

using ProjectStructure.Controllers;

using System.Linq;
using System.Collections.Generic;

using Date = System.DateTime;
using Task = System.Threading.Tasks.Task;

namespace Server.IntegratedTests.Controllers
{

    public class ProjectControllerTest : Helpers.ControllerTestBase
    {
        // TESTS
        #region CREATE_PROJECT
        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 2)]
        [InlineData(2, 1)]
        [InlineData(2, 2)]
        public async Task CreateProject_When_PassedRegularData_Then_InserNewProject(int userIdData, int teamIdData)
        {
            // Arrange
            ProjectsController controller = new ProjectsController(queryProcessor, commandProcessor)
            {
                ControllerContext = GetDefaultControllerContext
            };

            int userId = userIdData;
            int teamId = teamIdData;

            CreateProjectDTO projectDTO = new CreateProjectDTO()
            {
                Name = "New Project",
                Description = "Description",

                CreatedAt = Date.Now,
                Deadline = Date.Now.AddDays(25),
                
                TeamId = teamId,
                AuthorId = userId,
            };

            string[] projectsBeforeInserting = await binaryDbContext.Projects.Select(p => p.Name).ToArrayAsync();
            string[] usersProjectBeforeInserting = binaryDbContext.Users.Find(userId).Projects.Select(p => p.Name).ToArray();
            string[] teamProjectBeforeInserting = binaryDbContext.Teams.Find(teamId).Projects.Select(p => p.Name).ToArray();

            int projectsAmountBeforeInserting = await binaryDbContext.Projects.CountAsync();

            int expectedAfterInserting = projectsAmountBeforeInserting + 1;

            // Act
            CommandResponse response = await controller.Post(projectDTO);

            int actualAfterInserting = await binaryDbContext.Projects.CountAsync();
            string[] projectsAfterInserting = await binaryDbContext.Projects.Select(p => p.Name).ToArrayAsync();
            string[] usersProjectAfterInserting = binaryDbContext.Users.Find(userId).Projects.Select(p => p.Name).ToArray();
            string[] teamProjectAfterInserting = binaryDbContext.Teams.Find(teamId).Projects.Select(p => p.Name).ToArray();

            // Assert
            Assert.True(response.IsSucessed);
            Assert.Equal(expectedAfterInserting, actualAfterInserting);

            Assert.NotEmpty(projectsAfterInserting);
            Assert.NotEmpty(usersProjectAfterInserting);
            Assert.NotEmpty(teamProjectAfterInserting);

            Assert.DoesNotContain(projectDTO.Name, projectsBeforeInserting);
            Assert.Contains(projectDTO.Name, projectsAfterInserting);

            Assert.DoesNotContain(projectDTO.Name, usersProjectBeforeInserting);
            Assert.Contains(projectDTO.Name, usersProjectAfterInserting);

            Assert.DoesNotContain(projectDTO.Name, teamProjectBeforeInserting);
            Assert.Contains(projectDTO.Name, teamProjectAfterInserting);
        }
        [Fact]
        public async Task CreateProject_When_NullData_Then_Exception()
        {
            // Arrange
            ProjectsController controller = new ProjectsController(queryProcessor, commandProcessor)
            {
                ControllerContext = GetDefaultControllerContext
            };

            CreateProjectDTO projectDTO = null;

            int projectsAmountBeforeInserting = await binaryDbContext.Projects.CountAsync();

            int expectedAfterInserting = projectsAmountBeforeInserting;

            // Act
            await Assert.ThrowsAsync<System.ArgumentNullException>(async () => await controller.Post(projectDTO));

            int actualAfterInserting = await binaryDbContext.Projects.CountAsync();

            // Assert
            Assert.Equal(expectedAfterInserting, actualAfterInserting);
        }
        #endregion
        #region GET_TASK_PER_PROJECT
        [Fact]
        public async Task GetTaskPerProject_When_CorrectUserId_Then_CorretData()
        {

            // Arrange
            ProjectsController controller = new ProjectsController(queryProcessor, commandProcessor)
            {
                ControllerContext = GetDefaultControllerContext
            };

            int firstUserId = 1;

            IDictionary<string, int> expectedTaskPerProjectForFirstUser = new Dictionary<string, int>()
            {
                ["Project 1"] = 3
            };

            int secondUserId = 2;

            IDictionary<string, int> expectedTaskPerProjectForSecondUser = new Dictionary<string, int>()
            {
                ["Project 2"] = 1,
            };

            // Act
            IDictionary<string, int> actualTaskPerProjectForFirstUser = await controller.GetTasksAmountPerProject(firstUserId);
            IDictionary<string, int> actualTaskPerProjectForSecondUser = await controller.GetTasksAmountPerProject(secondUserId);


            // Assert
            Assert.NotEmpty(actualTaskPerProjectForFirstUser);
            Assert.Equal(expectedTaskPerProjectForFirstUser, actualTaskPerProjectForFirstUser);

            Assert.NotEmpty(actualTaskPerProjectForSecondUser);
            Assert.Equal(expectedTaskPerProjectForSecondUser, actualTaskPerProjectForSecondUser);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(50)]
        public async Task GetTaskPerProject_When_WrongUserId_Then_EmptyCollection(int userIdData)
        {
            // Arrange
            ProjectsController controller = new ProjectsController(queryProcessor, commandProcessor)
            {
                ControllerContext = GetDefaultControllerContext
            };

            int userId = userIdData;
            
            // Act
            IDictionary<string, int> actualTaskPerProjectForFirstUser = await controller.GetTasksAmountPerProject(userId);

            // Assert
            Assert.Empty(actualTaskPerProjectForFirstUser);
        }
        #endregion
    }
}
