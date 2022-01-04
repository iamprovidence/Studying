using Xunit;

using BusinessLayer.Test.Helpers;
using BusinessLayer.Queries.Query.Tasks;
using BusinessLayer.Queries.Handler.Tasks;

using DataAccessLayer.Interfaces;

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using TaskDb = DataAccessLayer.Entities.Task;

namespace BusinessLayer.Test.Queries.Tasks
{
    public class TaskQueryHandlerTest : IDisposable
    {
        // FIELDS
        DefaultFakeDataInitializer dataInitializer;
        IUnitOfWork unitOfWork;

        // CONSTRUCTORS
        public TaskQueryHandlerTest()
        {
            dataInitializer = new DefaultFakeDataInitializer();
            unitOfWork = DbFactory.CreateDefaultUnitOfWork(dataInitializer);
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }

        // TEST
        #region SHORT_TASKS
        [Theory]
        [MemberData(nameof(RandomDataDrivenProvider.RandomUserIdAndTaskLength), MemberType = typeof(RandomDataDrivenProvider))]
        public async Task ShortTaskQuery_When_CorrectId_Then_CorrectData(int userIdData, int maxTaskLengthData)
        {
            // Arrange
            TaskQueryHandler queryHandler = new TaskQueryHandler();
            queryHandler.SetUnitOfWork(unitOfWork);

            int userId = userIdData;
            int maxTaskLength = maxTaskLengthData;

            IEnumerable<int> expectedShortTask = dataInitializer.UsersTask[userId - 1].Where(t => t.Name.Length < maxTaskLength).Select(t => t.Id);

            // Act
            IEnumerable<int> actualShortTask = (await queryHandler.HandleAsync(new ShortTaskQuery(userId, maxTaskLength))).Select(t => t.Id);

            // Assert
            Assert.NotNull(actualShortTask);
            Assert.Equal(expectedShortTask, actualShortTask);
        }

        [Fact]
        public async Task ShortTaskQuery_When_WrongId_Then_EmptyCollection()
        {
            // Arrange
            TaskQueryHandler queryHandler = new TaskQueryHandler();
            queryHandler.SetUnitOfWork(unitOfWork);

            int userId = -1;

            // Act
            IEnumerable<TaskDb> actualShortTask = await queryHandler.HandleAsync(new ShortTaskQuery(userId));

            // Assert
            Assert.NotNull(actualShortTask);
            Assert.Empty(actualShortTask);
        }
        #endregion
        #region FINISHED_IN_YEAR_TASKS

        [Theory]
        [MemberData(nameof(RandomDataDrivenProvider.RandomUserIdAndYear), MemberType = typeof(RandomDataDrivenProvider))]
        public async Task FinishedInYearTasks_When_CorrectId_Then_CorrectData(int userIdData, int yearData)
        {
            // Arrange
            TaskQueryHandler queryHandler = new TaskQueryHandler();
            queryHandler.SetUnitOfWork(unitOfWork);

            int userId = userIdData;
            int year = yearData;

            IEnumerable<int> expectedFinishedTask = dataInitializer.UsersTask[userId - 1].Where(t => t.FinishedAt.Year == year).Select(t => t.Id);

            // Act
            IEnumerable<int> actualFinishedTask = (await queryHandler.HandleAsync(new FinishedInYearQuery(userId, year))).Select(t => t.Id);

            // Assert
            Assert.NotNull(actualFinishedTask);
            Assert.Equal(expectedFinishedTask, actualFinishedTask);
        }

        [Fact]
        public async Task FinishedTaskQuery_When_WrongId_Then_EmptyCollection()
        {
            // Arrange
            TaskQueryHandler queryHandler = new TaskQueryHandler();
            queryHandler.SetUnitOfWork(unitOfWork);

            int userId = -1;

            // Act
            IEnumerable<TaskDb> actualFinishedTask = await queryHandler.HandleAsync(new FinishedInYearQuery(userId));

            // Assert
            Assert.NotNull(actualFinishedTask);
            Assert.Empty(actualFinishedTask);
        }
        #endregion
    }
}
