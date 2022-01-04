using Xunit;

using BusinessLayer.Commands;

using Microsoft.EntityFrameworkCore;

using ProjectStructure.Controllers;

using Core.DataTransferObjects.Team;

using System.Linq;

using Task = System.Threading.Tasks.Task;
using Date = System.DateTime;

namespace Server.IntegratedTests.Controllers
{
    public class TeamControllerTest : Helpers.ControllerTestBase
    {
        #region CREATE_TEAM
        [Fact]
        public async Task CreateTeam_CorrectData_CreateNewTeam()
        {
            // Arrange
            TeamsController controller = new TeamsController(queryProcessor, commandProcessor)
            {
                ControllerContext = GetDefaultControllerContext
            };
            
            CreateTeamDTO teamDTO = new CreateTeamDTO()
            {
                Name = "New Team",
                CreatedAt = Date.Now,
            };

            int teamBeforeInsertion = await binaryDbContext.Teams.CountAsync();
            int expectedAfterInserting = teamBeforeInsertion + 1;

            string[] teamsBeforeInserting = await binaryDbContext.Teams.Select(t => t.Name).ToArrayAsync();
            
            // Act
            CommandResponse response = await controller.Post(teamDTO);

            int actualAfterInserting = await binaryDbContext.Teams.CountAsync();

            string[] teamsAfterInserting = await binaryDbContext.Teams.Select(t => t.Name).ToArrayAsync();

            // Assert
            Assert.True(response.IsSucessed);
            Assert.Equal(expectedAfterInserting, actualAfterInserting);

            Assert.NotEmpty(teamsAfterInserting);

            Assert.DoesNotContain(teamDTO.Name, teamsBeforeInserting);
            Assert.Contains(teamDTO.Name, teamsAfterInserting);
        }

        [Fact]
        public async Task CreateTeam_NullData_Exception()
        {
            // Arrange
            TeamsController controller = new TeamsController(queryProcessor, commandProcessor)
            {
                ControllerContext = GetDefaultControllerContext
            };

            CreateTeamDTO teamDTO = null;

            int teamBeforeInsertion = await binaryDbContext.Teams.CountAsync();
            int expectedAfterInserting = teamBeforeInsertion;

            string[] teamsBeforeInserting = await binaryDbContext.Teams.Select(t => t.Name).ToArrayAsync();

            // Act
            await Assert.ThrowsAsync<System.ArgumentNullException>(async () => await controller.Post(teamDTO));

            int actualAfterInserting = await binaryDbContext.Teams.CountAsync();

            string[] teamsAfterInserting = await binaryDbContext.Teams.Select(t => t.Name).ToArrayAsync();

            // Assert
            Assert.Equal(expectedAfterInserting, actualAfterInserting);

            Assert.Equal(teamsBeforeInserting, teamsAfterInserting);
        }
        #endregion
    }
}
