using Xunit;

using BusinessLayer.Test.Helpers;
using BusinessLayer.Queries.Query.Projects;
using BusinessLayer.Queries.Handler.Projects;

using DataAccessLayer.Interfaces;

using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BusinessLayer.Test.Queries.Projects
{
    public class ProjectQueryHandlerTest : IDisposable
    {
        // FIELDS
        DefaultFakeDataInitializer dataInitializer;
        IUnitOfWork unitOfWork;

        // CONSTRUCTORS
        public ProjectQueryHandlerTest()
        {
            dataInitializer = new DefaultFakeDataInitializer();
            unitOfWork = DbFactory.CreateDefaultUnitOfWork(dataInitializer);
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }
        
        // TEST
        [Theory]
        [MemberData(nameof(RandomDataDrivenProvider.RandomUserId), MemberType = typeof(RandomDataDrivenProvider))]
        public async Task TaskAmountPerProject_When_CorrectId_Then_CorrectData(int userIdData)
        {
            // Arrange
            ProjectQueryHandler queryHandler = new ProjectQueryHandler();
            queryHandler.SetUnitOfWork(unitOfWork);

            int userId = userIdData;

            IDictionary<string, int> expectedProjectTaskAmount = dataInitializer.ProjectTaskAmountPerUsers[userId - 1];

            // Act
            IDictionary<string, int> actualProjectTaskAmount = await queryHandler.HandleAsync(new TasksAmountPerProjectQuery(userId));


            // Assert
            Assert.NotNull(actualProjectTaskAmount);
            Assert.Equal(expectedProjectTaskAmount, actualProjectTaskAmount);
        }

        [Fact]
        public async Task TaskAmountPerProject_When_WrongId_Then_EmptyCollection()
        {
            // Arrange
            ProjectQueryHandler queryHandler = new ProjectQueryHandler();
            queryHandler.SetUnitOfWork(unitOfWork);

            int userId = -1;

            // Act
            IDictionary<string, int> actualProjectTaskAmount = await queryHandler.HandleAsync(new TasksAmountPerProjectQuery(userId));


            // Assert
            Assert.NotNull(actualProjectTaskAmount);
            Assert.Empty(actualProjectTaskAmount);
        }
    }
}
