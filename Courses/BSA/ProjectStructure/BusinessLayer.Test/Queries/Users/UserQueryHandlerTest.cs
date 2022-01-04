using Xunit;

using BusinessLayer.Test.Helpers;
using BusinessLayer.Queries.Query.Users;
using BusinessLayer.Queries.Handler.Users;
using BusinessLayer.DataTransferObjects;

using DataAccessLayer.Interfaces;

using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BusinessLayer.Test.Queries.Users
{
    public class UserQueryHandlerTest : IDisposable
    {
        // FIELDS
        DefaultFakeDataInitializer dataInitializer;
        IUnitOfWork unitOfWork;

        // CONSTRUCTORS
        public UserQueryHandlerTest()
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
        public async Task OrderedUsersAndTherTasks_When_PassQuery_Then_CorrectData()
        {
            // Arrange
            UserQueryHandler queryHandler = new UserQueryHandler();
            queryHandler.SetUnitOfWork(unitOfWork);

            IEnumerable<UserTasksDTO> expectedUserTask = dataInitializer.UsersAndTasks;

            // Act
            IEnumerable<UserTasksDTO> actualUserTask = await queryHandler.HandleAsync(new OrderedUserQuery());

            // Assert
            Assert.NotNull(actualUserTask);
            Assert.Equal(expectedUserTask, actualUserTask);
        }

        [Fact]
        public async Task OrderedUsersAndTherTasks_When_PassNull_Then_Exception()
        {
            // Arrange
            UserQueryHandler queryHandler = new UserQueryHandler();
            queryHandler.SetUnitOfWork(unitOfWork);

            // Act
            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await queryHandler.HandleAsync((OrderedUserQuery)null));
        }
    }
}
