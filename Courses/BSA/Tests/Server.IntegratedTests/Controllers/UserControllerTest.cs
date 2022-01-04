using Xunit;

using BusinessLayer.Commands;

using Microsoft.EntityFrameworkCore;

using ProjectStructure.Controllers;

using DataAccessLayer.Entities;

using System.Linq;

using Task = System.Threading.Tasks.Task;

namespace Server.IntegratedTests.Controllers
{
    public class UserControllerTest : Helpers.ControllerTestBase
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task DeleteUser_When_CorrectId_Then_DeleteUserAndDataCascade(int userIdData)
        {
            // Arrange
            UsersController controller = new UsersController(queryProcessor, commandProcessor)
            {
                ControllerContext = GetDefaultControllerContext
            };

            int userId = userIdData;

            User expectedUserToDelete = await binaryDbContext.Users.FindAsync(userId);

            int usersAmountBeforeDeleting = await binaryDbContext.Users.CountAsync();

            string[] usersBeforeDeleting = await binaryDbContext.Users.Select(u => u.FirstName).ToArrayAsync();
            string[] usersTasksBeforeDelete = await binaryDbContext.Tasks.Where(t => t.PerformerId == userId).Select(t => t.Name).ToArrayAsync();
            string[] usersProjectBeforeDelete = await binaryDbContext.Projects.Where(p => p.AuthorId == userId).Select(p => p.Name).ToArrayAsync();
            string[] usersInTeamBeforeDelete = (await binaryDbContext.Teams.FirstAsync(t => t.Id == userId)).Users.Select(u => u.FirstName).ToArray();


            int expectedUserAmountAfterDeleting = usersAmountBeforeDeleting - 1;

            // Act
            CommandResponse response = await controller.Delete(userId);

            int actualUserAmountAfterDeleting = await binaryDbContext.Users.CountAsync();

            string[] usersAfterDelete = await binaryDbContext.Users.Select(u => u.FirstName).ToArrayAsync();
            string[] usersTasksAfterDelete = await binaryDbContext.Tasks.Where(t => t.PerformerId == userId).Select(t => t.Name).ToArrayAsync();
            string[] usersProjectAfterDelete = await binaryDbContext.Projects.Where(p => p.AuthorId == userId).Select(p => p.Name).ToArrayAsync();
            string[] usersInTeamAfterDelete = (await binaryDbContext.Teams.FirstAsync(t => t.Id == userId)).Users.Select(u => u.FirstName).ToArray();

            // Assert
            Assert.True(response.IsSucessed);

            Assert.Equal(expectedUserAmountAfterDeleting, actualUserAmountAfterDeleting);
            
            Assert.Contains(expectedUserToDelete.FirstName, usersBeforeDeleting);
            Assert.DoesNotContain(expectedUserToDelete.FirstName, usersAfterDelete);

            Assert.NotEqual(usersTasksBeforeDelete.Length, usersTasksAfterDelete.Length);
            Assert.NotEqual(usersTasksBeforeDelete, usersTasksAfterDelete);
            Assert.NotEmpty(usersTasksBeforeDelete);
            Assert.Empty(usersTasksAfterDelete);

            Assert.NotEqual(usersProjectBeforeDelete.Length, usersProjectAfterDelete.Length);
            Assert.NotEqual(usersProjectBeforeDelete, usersProjectAfterDelete);
            Assert.NotEmpty(usersProjectBeforeDelete);
            Assert.Empty(usersProjectAfterDelete);

            Assert.NotEqual(usersInTeamBeforeDelete.Length, usersInTeamAfterDelete.Length);
            Assert.NotEqual(usersInTeamBeforeDelete, usersInTeamAfterDelete);
            Assert.NotEmpty(usersInTeamBeforeDelete);
            Assert.Empty(usersInTeamAfterDelete);
        }

        [Theory]
        [InlineData(50)]
        [InlineData(-1)]
        public async Task DeleteUser_When_WrongId_Then_DoNotDelete(int userIdData)
        {
            // Arrange
            UsersController controller = new UsersController(queryProcessor, commandProcessor)
            {
                ControllerContext = GetDefaultControllerContext
            };

            int userId = userIdData;            

            int usersAmountBeforeDeleting = await binaryDbContext.Users.CountAsync();

            int expectedUserAmountAfterDeleting = usersAmountBeforeDeleting;

            // Act
            CommandResponse response = await controller.Delete(userId);

            int actualUserAmountAfterDeleting = await binaryDbContext.Users.CountAsync();            

            // Assert
            Assert.False(response.IsSucessed);

            Assert.Equal(expectedUserAmountAfterDeleting, actualUserAmountAfterDeleting);
        }
    }
}
