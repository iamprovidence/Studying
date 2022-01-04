using BusinessLayer.DataTransferObjects;
using BusinessLayer.Queries.Handler.Teams;
using BusinessLayer.Queries.Query.Teams;
using BusinessLayer.Test.Helpers;

using DataAccessLayer.Interfaces;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xunit;

namespace BusinessLayer.Test.Queries.Teams
{
    public class TeamQueryHandlerTest : IDisposable
    {
        // FIELDS
        DefaultFakeDataInitializer dataInitializer;
        IUnitOfWork unitOfWork;

        // CONSTRUCTORS
        public TeamQueryHandlerTest()
        {
            dataInitializer = new DefaultFakeDataInitializer();
            unitOfWork = DbFactory.CreateDefaultUnitOfWork(dataInitializer);
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }

        // TEST
        [Fact]
        public async Task TeamByAgeLimit_When_CorrectData_Then_CorrectResult()
        {
            // Arrange
            TeamQueryHandler queryHandler = new TeamQueryHandler();
            queryHandler.SetUnitOfWork(unitOfWork);

            int participantAge = 10;
            IEnumerable<TeamUsersDTO> expectedTeamUsers = dataInitializer.TeamUsers;

            // Act
            IEnumerable<TeamUsersDTO> actulTeamUsers = await queryHandler.HandleAsync(new TeamByAgeLimitQuery(participantAge));

            // Assert
            Assert.NotNull(actulTeamUsers);
            Assert.NotEmpty(actulTeamUsers);
            Assert.Equal(expectedTeamUsers, actulTeamUsers);
        }
        [Fact]
        public async Task TeamByAgeLimit_When_WrongAge_Then_EmptyCollection()
        {
            // Arrange
            TeamQueryHandler queryHandler = new TeamQueryHandler();
            queryHandler.SetUnitOfWork(unitOfWork);

            int participantAge = 25;

            // Act
            IEnumerable<TeamUsersDTO> actulTeamUsers = await queryHandler.HandleAsync(new TeamByAgeLimitQuery(participantAge));

            // Assert
            Assert.NotNull(actulTeamUsers);
            Assert.Empty(actulTeamUsers);
        }
    }
}
