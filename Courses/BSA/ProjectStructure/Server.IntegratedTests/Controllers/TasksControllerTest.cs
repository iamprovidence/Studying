using Xunit;

using BusinessLayer.Commands;

using Microsoft.EntityFrameworkCore;

using ProjectStructure.Controllers;

using System.Linq;

using Task = System.Threading.Tasks.Task;
using TaskDb = DataAccessLayer.Entities.Task;

namespace Server.IntegratedTests.Controllers
{
    public class TasksControllerTest : Helpers.ControllerTestBase
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task DeleteTask_When_CorrectId_Then_DeleteCascade(int taskIdData)
        {
            // Arrange
            TasksController controller = new TasksController(queryProcessor, commandProcessor)
            {
                ControllerContext = GetDefaultControllerContext
            };

            int taskId = taskIdData;

            TaskDb expectedTaskToDelete = await binaryDbContext.Tasks.FindAsync(taskId);

            int tasksAmountBeforeDeleting = await binaryDbContext.Tasks.CountAsync();

            string[] taskBeforeDeleting = await binaryDbContext.Tasks.Select(t => t.Name).ToArrayAsync();
            string[] usersTaskBeforeDeleting = binaryDbContext.Users.First(u => u.Id == expectedTaskToDelete.PerformerId).Tasks.Select(t => t.Name).ToArray();
            string[] projectTasksBeforeDelete = binaryDbContext.Projects.First(p => p.Id == expectedTaskToDelete.ProjectId).Tasks.Select(t => t.Name).ToArray();

            int expectedTasksAmountAfterDeleting = tasksAmountBeforeDeleting - 1;

            // Act
            CommandResponse response = await controller.Delete(taskId);

            int actualTasksAmountAfterDeleting = await binaryDbContext.Tasks.CountAsync();

            string[] taskAfterDeleting = await binaryDbContext.Tasks.Select(t => t.Name).ToArrayAsync();
            string[] usersTaskAfterDeleting = binaryDbContext.Users.First(u => u.Id == expectedTaskToDelete.PerformerId).Tasks.Select(t => t.Name).ToArray();
            string[] projectTasksAfterDelete = binaryDbContext.Projects.First(p => p.Id == expectedTaskToDelete.ProjectId).Tasks.Select(t => t.Name).ToArray();

            // Assert
            Assert.True(response.IsSucessed);

            Assert.Equal(expectedTasksAmountAfterDeleting, actualTasksAmountAfterDeleting);

            Assert.Contains(expectedTaskToDelete.Name, taskBeforeDeleting);
            Assert.DoesNotContain(expectedTaskToDelete.Name, taskAfterDeleting);
            Assert.NotEqual(taskBeforeDeleting.Length, taskAfterDeleting.Length);
            Assert.NotEqual(taskBeforeDeleting, taskAfterDeleting);

            Assert.Contains(expectedTaskToDelete.Name, usersTaskBeforeDeleting);
            Assert.DoesNotContain(expectedTaskToDelete.Name, usersTaskAfterDeleting);
            Assert.NotEqual(usersTaskBeforeDeleting.Length, usersTaskAfterDeleting.Length);
            Assert.NotEqual(usersTaskBeforeDeleting, usersTaskAfterDeleting);

            Assert.Contains(expectedTaskToDelete.Name, projectTasksBeforeDelete);
            Assert.DoesNotContain(expectedTaskToDelete.Name, projectTasksAfterDelete);
            Assert.NotEqual(projectTasksBeforeDelete.Length, projectTasksAfterDelete.Length);
            Assert.NotEqual(projectTasksBeforeDelete, projectTasksAfterDelete);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(50)]
        public async Task DeleteTask_When_WrongId_Then_DoNotDelete(int taskIdData)
        {
            // Arrange
            TasksController controller = new TasksController(queryProcessor, commandProcessor)
            {
                ControllerContext = GetDefaultControllerContext
            };

            int taskId = taskIdData;
            
            int tasksAmountBeforeDeleting = await binaryDbContext.Tasks.CountAsync();

            string[] taskBeforeDeleting = await binaryDbContext.Tasks.Select(t => t.Name).ToArrayAsync();

            int expectedTasksAmountAfterDeleting = tasksAmountBeforeDeleting;

            // Act
            CommandResponse response = await controller.Delete(taskId);

            int actualTasksAmountAfterDeleting = await binaryDbContext.Tasks.CountAsync();

            string[] taskAfterDeleting = await binaryDbContext.Tasks.Select(t => t.Name).ToArrayAsync();

            // Assert
            Assert.False(response.IsSucessed);

            Assert.Equal(expectedTasksAmountAfterDeleting, actualTasksAmountAfterDeleting);

            Assert.Equal(taskBeforeDeleting.Length, taskAfterDeleting.Length);
            Assert.Equal(taskBeforeDeleting, taskAfterDeleting);
        }
    }
}
