using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using DataAccessLayer.Test.TestData;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Xunit;

using TaskDb = DataAccessLayer.Entities.Task;
using Task = System.Threading.Tasks.Task;
using Date = System.DateTime;

namespace DataAccessLayer.Test.Repositories
{
    public class TaskRepositoryTest : IDisposable
    {
        // FIELDS
        BinaryDbContext testContext;
        TaskRepositoryFakeDataInitializer testData;

        // CONSTRUCTORS
        public TaskRepositoryTest()
        {
            testData = new TaskRepositoryFakeDataInitializer();
            testContext = new BinaryDbContext(CreateNewContextOptions(), testData);
            testContext.Database.EnsureCreated();
        }

        private DbContextOptions<BinaryDbContext> CreateNewContextOptions()
        {
            // Create a fresh service provider, and therefore a fresh 
            // InMemory database instance.
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            return new DbContextOptionsBuilder<BinaryDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .UseInternalServiceProvider(serviceProvider)
                .Options;
        }
        public void Dispose()
        {
            testContext.Dispose();
        }

        // PROPERTIES
        TaskRepositoryFakeDataInitializer TestData => testData;

        // TESTS
        #region COUNT
        [Fact]
        public async Task CountTest()
        {
            // Arrange
            TaskRepository repository = new TaskRepository(testContext);

            int userId = 1;
            int projectId = 1;
            int statusId = 1;

            int expectedEntitiesCount = TestData.TotalEntitiesCount;
            int expectedEntitiesFromUserPoject = TestData.TotalUserProjectAmount;
            int expectedEntitiesFromUserPojectStatus = TestData.TotalUserProjectStatusAmount;
            int expectedWrongCount = 0;

            // Act
            int actualEntitiesCount = await repository.CountAsync();
            int actualWrongCount = await repository.CountAsync(p => p.Name.Contains("Wrong"));
            int actualEntitiesFromUserPoject = await repository.CountAsync(userId, projectId);
            int actualEntitiesFromUserPojectStatus = await repository.CountExceptAsync(userId, statusId);

            // Assert
            Assert.Equal(expectedEntitiesCount, actualEntitiesCount);
            Assert.Equal(expectedWrongCount, actualWrongCount);
            Assert.Equal(expectedEntitiesFromUserPoject, expectedEntitiesFromUserPoject);
            Assert.Equal(expectedEntitiesFromUserPojectStatus, expectedEntitiesFromUserPojectStatus);

            await Assert.ThrowsAsync<ArgumentNullException>(() => repository.CountAsync(null));
        }
        #endregion

        #region GET
        [Fact]
        public async Task GetById()
        {
            // Arrange
            TaskRepository repository = new TaskRepository(testContext);

            TaskDb expectedEntity = testContext.Tasks.First();
            int entityId = expectedEntity.Id;

            int wrongId = -1;

            // Act
            TaskDb actualEntity = await repository.GetAsync(entityId);
            TaskDb actualNullResult = await repository.GetAsync(wrongId);

            // Assert
            Assert.Equal(expectedEntity, actualEntity);
            Assert.Null(actualNullResult);
        }

        [Fact]
        public async Task GetWithLongestDescription()
        {
            // Arrange
            TaskRepository repository = new TaskRepository(testContext);

            int projectId = 2;
            int wrongProjectId = -1;

            TaskDb expectedEntity = TestData.WithLongestDescriptionForSecondProject;

            // Act
            TaskDb actualEntity = await repository.GetWithLongestDescriptionAsync(projectId);
            TaskDb actualNullResult = await repository.GetWithLongestDescriptionAsync(wrongProjectId);

            // Assert
            Assert.Equal(expectedEntity.Id, actualEntity.Id);
            Assert.Null(actualNullResult);
        }

        [Fact]
        public async Task GetWithShortestName()
        {
            // Arrange
            TaskRepository repository = new TaskRepository(testContext);

            int projectId = 2;
            int wrongProjectId = -1;

            TaskDb expectedEntity = TestData.WithShortestNameForSecondUser;

            // Act
            TaskDb actualEntity = await repository.GetWithShortestNameAsync(projectId);
            TaskDb actualNullResult = await repository.GetWithShortestNameAsync(wrongProjectId);

            // Assert
            Assert.Equal(expectedEntity.Id, actualEntity.Id);
            Assert.Null(actualNullResult);
        }

        [Fact]
        public async Task GetLongestTask()
        {
            // Arrange
            TaskRepository repository = new TaskRepository(testContext);

            int userId = 1;
            int wrongUserId = -1;

            TaskDb expectedEntity = TestData.LongestTask;

            // Act
            TaskDb actualEntity = await repository.GetLongestTaskAsync(userId);
            TaskDb actualNullResult = await repository.GetLongestTaskAsync(wrongUserId);

            // Assert
            Assert.Equal(expectedEntity.Id, actualEntity.Id);
            Assert.Null(actualNullResult);
        }
        [Fact]
        public async Task GetByPredicate_When_CorrectPredicate_Then_GetCorrectResult()
        {
            // Arrange
            TaskRepository repository = new TaskRepository(testContext);

            int expectedEntityCount = TestData.TotalEntitiesCount;
            Expression<Func<TaskDb, bool>> predicateToGet = p => true;
            IEnumerable<TaskDb> expectedEntities = testContext.Tasks.Where(predicateToGet);

            // Act
            IEnumerable<TaskDb> actualEntities = await repository.GetAsync(predicateToGet);
            int actualEntitiesCount = actualEntities.Count();

            // Assert
            Assert.Equal(expectedEntities, actualEntities);
            Assert.Equal(expectedEntityCount, actualEntitiesCount);
        }

        [Fact]
        public async Task GetByPredicate_When_NullPredicate_Then_GetAllData()
        {
            // Arrange
            TaskRepository repository = new TaskRepository(testContext);

            int expectedEntityCount = TestData.TotalEntitiesCount;
            Expression<Func<TaskDb, bool>> predicateToGet = null;
            IEnumerable<TaskDb> expectedEntities = testContext.Tasks;

            // Act
            IEnumerable<TaskDb> actualEntities = await repository.GetAsync(predicateToGet);
            int actualEntitiesCount = actualEntities.Count();

            // Assert
            Assert.Equal(expectedEntities, actualEntities);
            Assert.Equal(expectedEntityCount, actualEntitiesCount);
        }
        #endregion
        #region DELETE
        [Fact]
        public async Task DeleteById_When_CorrectId_Then_CorrectDeleting()
        {
            // Arrange
            TaskRepository repository = new TaskRepository(testContext);

            TaskDb entityToDelete = testContext.Tasks.First();
            int idToDelete = entityToDelete.Id;
            int expectedCountBeforeDelete = TestData.TotalEntitiesCount;
            int expectedCountAfterDelete = TestData.TotalEntitiesCount - 1;

            // Act
            int actualCountBeforeDelete = testContext.Tasks.Count();
            await repository.DeleteAsync(idToDelete);
            testContext.SaveChanges();
            int actualCountAfterDelete = testContext.Tasks.Count();

            // Assert
            Assert.Equal(expectedCountBeforeDelete, actualCountBeforeDelete);
            Assert.Equal(expectedCountAfterDelete, actualCountAfterDelete);
            Assert.DoesNotContain(testContext.Users, x => x.Id == idToDelete);
            Assert.DoesNotContain(entityToDelete, testContext.Tasks);
        }
        [Fact]
        public async Task DeleteById_When_WrongId_Then_DoNotDelete()
        {
            // Arrange
            TaskRepository repository = new TaskRepository(testContext);

            int idToDelete = -1;
            int expectedCountBeforeDelete = TestData.TotalEntitiesCount;
            int expectedCountAfterDelete = TestData.TotalEntitiesCount;

            // Act
            int actualCountBeforeDelete = testContext.Tasks.Count();
            await repository.DeleteAsync(idToDelete);
            testContext.SaveChanges();
            int actualCountAfterDelete = testContext.Tasks.Count();

            // Assert
            Assert.Equal(expectedCountBeforeDelete, actualCountBeforeDelete);
            Assert.Equal(expectedCountAfterDelete, actualCountAfterDelete);
        }
        [Fact]
        public async Task DeleteByObject_When_CorrectObject_Then_Delete()
        {
            // Arrange
            TaskRepository repository = new TaskRepository(testContext);

            TaskDb entityToDelete = testContext.Tasks.First();
            int expectedCountBeforeDelete = TestData.TotalEntitiesCount;
            int expectedCountAfterDelete = TestData.TotalEntitiesCount - 1;

            // Act
            int actualCountBeforeDelete = testContext.Tasks.Count();
            await repository.DeleteAsync(entityToDelete);
            testContext.SaveChanges();
            int actualCountAfterDelete = testContext.Tasks.Count();

            // Assert
            Assert.Equal(expectedCountBeforeDelete, actualCountBeforeDelete);
            Assert.Equal(expectedCountAfterDelete, actualCountAfterDelete);
            Assert.DoesNotContain(entityToDelete, testContext.Tasks);
        }

        [Fact]
        public async Task DeleteByObject_When_NullObject_Then_DoNotDelete()
        {
            // Arrange
            TaskRepository repository = new TaskRepository(testContext);

            TaskDb entityToDelete = null;
            int expectedCountBeforeDelete = TestData.TotalEntitiesCount;
            int expectedCountAfterDelete = TestData.TotalEntitiesCount;

            // Act
            int actualCountBeforeDelete = testContext.Tasks.Count();
            await repository.DeleteAsync(entityToDelete);
            testContext.SaveChanges();
            int actualCountAfterDelete = testContext.Tasks.Count();

            // Assert
            Assert.Equal(expectedCountBeforeDelete, actualCountBeforeDelete);
            Assert.Equal(expectedCountAfterDelete, actualCountAfterDelete);
        }

        [Fact]
        public async Task DeleteByObject_When_NewObjectObject_Then_Exception()
        {
            // Arrange
            TaskRepository repository = new TaskRepository(testContext);

            TaskDb entityToDelete = new TaskDb { Id = 10, Name = "Task 10", Description = "Descrшзешщт", CreatedAt = Date.Now, FinishedAt = Date.Now.AddMonths(5) };
            int expectedCountBeforeDelete = TestData.TotalEntitiesCount;
            int expectedCountAfterDelete = TestData.TotalEntitiesCount;

            // Act
            int actualCountBeforeDelete = testContext.Tasks.Count();
            await repository.DeleteAsync(entityToDelete);
            int actualCountAfterDelete = testContext.Tasks.Count();

            // Assert
            Assert.Throws<DbUpdateConcurrencyException>(() => testContext.SaveChanges());

            Assert.Equal(expectedCountBeforeDelete, actualCountBeforeDelete);
            Assert.Equal(expectedCountAfterDelete, actualCountAfterDelete);
        }
        #endregion
        #region INSERT
        [Fact]
        public async Task Insert_When_CorrectData_Then_Insert()
        {
            // Arrange
            TaskRepository repository = new TaskRepository(testContext);

            int expectedEntitiesBeforeInsertCount = TestData.TotalEntitiesCount;
            int expectedEntitiesAfterInsertCount = TestData.TotalEntitiesCount + 1;

            TaskDb entityToInsert = new TaskDb { Id = 10, Name = "Task 10", Description = "Descrшзешщт", CreatedAt = Date.Now, FinishedAt = Date.Now.AddMonths(5) };

            // Act
            int actualEntitiesBeforeInsertCount = testContext.Tasks.Count();
            await repository.InsertAsync(entityToInsert);
            testContext.SaveChanges();
            int actualEntitiesAfterInsertCount = testContext.Tasks.Count();

            // Assert
            Assert.Equal(expectedEntitiesBeforeInsertCount, actualEntitiesBeforeInsertCount);
            Assert.Equal(expectedEntitiesAfterInsertCount, actualEntitiesAfterInsertCount);
            Assert.Contains(entityToInsert, testContext.Tasks);
        }

        [Fact]
        public async Task Insert_When_AlreadyExistedData_Then_Insert()
        {
            // Arrange
            TaskRepository repository = new TaskRepository(testContext);

            int expectedEntitiesBeforeInsertCount = TestData.TotalEntitiesCount;
            int expectedEntitiesAfterInsertCount = TestData.TotalEntitiesCount;

            TaskDb entityToInsert = testContext.Tasks.First();

            // Act
            int actualEntitiesBeforeInsertCount = testContext.Tasks.Count();
            await repository.InsertAsync(entityToInsert);
            Assert.Throws<ArgumentException>(() => testContext.SaveChanges());
            int actualEntitiesAfterInsertCount = testContext.Tasks.Count();

            // Assert
            Assert.Equal(expectedEntitiesBeforeInsertCount, actualEntitiesBeforeInsertCount);
            Assert.Equal(expectedEntitiesAfterInsertCount, actualEntitiesAfterInsertCount);
            Assert.Contains(entityToInsert, testContext.Tasks);
        }
        #endregion
        #region UpdateTaskState
        [Theory]
        [InlineData(1, 3)]
        [InlineData(2, 4)]
        [InlineData(2, 8)]
        public void UpdateTaskState_When_CorrectData_Then_Update(int taskStateIdData, int newStateDate)
        {
            // Arrange
            UnitOfWork unitOfWork = new UnitOfWork(testContext);
            TaskRepository repository = new TaskRepository(testContext);

            int previousState = taskStateIdData;
            int newState = newStateDate;
            
            TaskDb entityToUpdate = testContext.Tasks.First(t => t.TaskStateId == previousState);
            int entityId = entityToUpdate.Id;

            int countPreviousStateBeforeUpdate  = testContext.Tasks.Count(t => t.TaskStateId == previousState);
            int countNewStateBeforeUpdate       = testContext.Tasks.Count(t => t.TaskStateId == newState);

            int expectedCountPreviousStateAfterUpdate = countPreviousStateBeforeUpdate - 1;
            int expectedCountNewStateAfterUpdate      = countNewStateBeforeUpdate + 1;

            // Act
            entityToUpdate.TaskStateId = newState;
            unitOfWork.Update(entityToUpdate);
            testContext.SaveChanges();

            TaskDb entityAfterUpdate = testContext.Tasks.Find(entityId);

            int actualCountPreviousStateAfterUpdate = testContext.Tasks.Count(t => t.TaskStateId == previousState);
            int actualCountNewStateAfterUpdate      = testContext.Tasks.Count(t => t.TaskStateId == newState);

            // Assert
            Assert.Equal(entityToUpdate.Id, entityAfterUpdate.Id);
            Assert.Equal(entityAfterUpdate.TaskStateId, newState);
            Assert.Equal(expectedCountPreviousStateAfterUpdate, actualCountPreviousStateAfterUpdate);
            Assert.Equal(expectedCountNewStateAfterUpdate, actualCountNewStateAfterUpdate);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        public void UpdateTaskState_When_TheSameData_Then_DoNothing(int taskStateIdData, int newStateDate)
        {
            // Arrange
            UnitOfWork unitOfWork = new UnitOfWork(testContext);
            TaskRepository repository = new TaskRepository(testContext);

            int previousState = taskStateIdData;
            int newState = newStateDate;

            TaskDb entityToUpdate = testContext.Tasks.First(t => t.TaskStateId == previousState);
            int entityId = entityToUpdate.Id;

            int countPreviousStateBeforeUpdate = testContext.Tasks.Count(t => t.TaskStateId == previousState);
            int countNewStateBeforeUpdate = testContext.Tasks.Count(t => t.TaskStateId == newState);

            int expectedCountPreviousStateAfterUpdate = countPreviousStateBeforeUpdate;
            int expectedCountNewStateAfterUpdate = countNewStateBeforeUpdate;

            // Act
            entityToUpdate.TaskStateId = newState;
            unitOfWork.Update(entityToUpdate);
            testContext.SaveChanges();

            TaskDb entityAfterUpdate = testContext.Tasks.FirstOrDefault(t => t.Id == entityId);

            int actualCountPreviousStateAfterUpdate = testContext.Tasks.Count(t => t.TaskStateId == previousState);
            int actualCountNewStateAfterUpdate = testContext.Tasks.Count(t => t.TaskStateId == newState);

            // Assert
            Assert.Equal(entityToUpdate.Id, entityAfterUpdate.Id);
            Assert.Equal(entityAfterUpdate.TaskStateId, newState);
            Assert.Equal(expectedCountPreviousStateAfterUpdate, actualCountPreviousStateAfterUpdate);
            Assert.Equal(expectedCountNewStateAfterUpdate, actualCountNewStateAfterUpdate);
        }
        #endregion
    }
}
