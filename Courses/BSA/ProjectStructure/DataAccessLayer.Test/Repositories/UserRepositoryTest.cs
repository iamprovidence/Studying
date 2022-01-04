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

using Task = System.Threading.Tasks.Task;
using Date = System.DateTime;

namespace DataAccessLayer.Test.Repositories
{
    public class UserRepositoryTest : IDisposable
    {
        // FIELDS
        BinaryDbContext testContext;
        UserRepositoryFakeDataInitializer testData;

        // CONSTRUCTORS
        public UserRepositoryTest()
        {
            testData = new UserRepositoryFakeDataInitializer();
            testContext = new BinaryDbContext(CreateNewContextOptions(), testData);
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
        UserRepositoryFakeDataInitializer TestData => testData;

        // TESTS
        #region COUNT
        [Fact]
        public async Task CountTest()
        {
            // Arrange
            UserRepository repository = new UserRepository(testContext);

            int expectedEntitiesCount = TestData.TotalEntitiesCount;
            int expectedWrongCount   = 0;

            // Act
            int actualEntitiesCount = await repository.CountAsync();
            int actualWrongCount = await repository.CountAsync(p => p.FirstName.Contains("Wrong"));

            // Assert
            Assert.Equal(expectedEntitiesCount, actualEntitiesCount);
            Assert.Equal(expectedWrongCount, actualWrongCount);

            await Assert.ThrowsAsync<ArgumentNullException>(() => repository.CountAsync(null));
        }
        #endregion

        #region GET
        [Fact]
        public async Task GetById()
        {
            // Arrange
            UserRepository repository = new UserRepository(testContext);

            User expectedEntity = testContext.Users.First();
            int entityId = expectedEntity.Id;

            int wrongId = -1;

            // Act
            User actualEntity = await repository.GetAsync(entityId);
            User actualNullResult = await repository.GetAsync(wrongId);

            // Assert
            Assert.Equal(expectedEntity, actualEntity);
            Assert.Null(actualNullResult);
        }
        [Fact]
        public async Task GetByPredicate_When_CorrectPredicate_Then_GetCorrectResult()
        {
            // Arrange
            UserRepository repository = new UserRepository(testContext);

            int expectedEntityCount = TestData.TotalEntitiesCount;
            Expression<Func<User, bool>> predicateToGet = p => true;
            IEnumerable<User> expectedEntities = testContext.Users.Where(predicateToGet);

            // Act
            IEnumerable<User> actualEntities = await repository.GetAsync(predicateToGet);
            int actualEntitiesCount = actualEntities.Count();

            // Assert
            Assert.Equal(expectedEntities, actualEntities);
            Assert.Equal(expectedEntityCount, actualEntitiesCount);
        }

        [Fact]
        public async Task GetByPredicate_When_NullPredicate_Then_GetAllData()
        {
            // Arrange
            UserRepository repository = new UserRepository(testContext);

            int expectedEntityCount = TestData.TotalEntitiesCount;
            Expression<Func<User, bool>> predicateToGet = null;
            IEnumerable<User> expectedEntities = testContext.Users;

            // Act
            IEnumerable<User> actualEntities = await repository.GetAsync(predicateToGet);
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
            UserRepository repository = new UserRepository(testContext);

            User entityToDelete = testContext.Users.First();
            int idToDelete = entityToDelete.Id;
            int expectedCountBeforeDelete = TestData.TotalEntitiesCount;
            int expectedCountAfterDelete = TestData.TotalEntitiesCount - 1;

            // Act
            int actualCountBeforeDelete = testContext.Users.Count();
            await repository.DeleteAsync(idToDelete);
            testContext.SaveChanges();
            int actualCountAfterDelete = testContext.Users.Count();

            // Assert
            Assert.Equal(expectedCountBeforeDelete, actualCountBeforeDelete);
            Assert.Equal(expectedCountAfterDelete, actualCountAfterDelete);
            Assert.DoesNotContain(testContext.Users, x => x.Id == idToDelete);
            Assert.DoesNotContain(entityToDelete, testContext.Users);
        }
        [Fact]
        public async Task DeleteById_When_WrongId_Then_DoNotDelete()
        {
            // Arrange
            UserRepository repository = new UserRepository(testContext);

            int idToDelete = -1;
            int expectedCountBeforeDelete = TestData.TotalEntitiesCount;
            int expectedCountAfterDelete = TestData.TotalEntitiesCount;

            // Act
            int actualCountBeforeDelete = testContext.Users.Count();
            await repository.DeleteAsync(idToDelete);
            testContext.SaveChanges();
            int actualCountAfterDelete = testContext.Users.Count();

            // Assert
            Assert.Equal(expectedCountBeforeDelete, actualCountBeforeDelete);
            Assert.Equal(expectedCountAfterDelete, actualCountAfterDelete);
        }
        [Fact]
        public async Task DeleteByObject_When_CorrectObject_Then_Delete()
        {
            // Arrange
            UserRepository repository = new UserRepository(testContext);

            User entityToDelete = testContext.Users.First();
            int expectedCountBeforeDelete = TestData.TotalEntitiesCount;
            int expectedCountAfterDelete = TestData.TotalEntitiesCount - 1;

            // Act
            int actualCountBeforeDelete = testContext.Users.Count();
            await repository.DeleteAsync(entityToDelete);
            testContext.SaveChanges();
            int actualCountAfterDelete = testContext.Users.Count();

            // Assert
            Assert.Equal(expectedCountBeforeDelete, actualCountBeforeDelete);
            Assert.Equal(expectedCountAfterDelete, actualCountAfterDelete);
            Assert.DoesNotContain(entityToDelete, testContext.Users);
        }

        [Fact]
        public async Task DeleteByObject_When_NullObject_Then_DoNotDelete()
        {
            // Arrange
            UserRepository repository = new UserRepository(testContext);

            User entityToDelete = null;
            int expectedCountBeforeDelete = TestData.TotalEntitiesCount;
            int expectedCountAfterDelete = TestData.TotalEntitiesCount;

            // Act
            int actualCountBeforeDelete = testContext.Users.Count();
            await repository.DeleteAsync(entityToDelete);
            testContext.SaveChanges();
            int actualCountAfterDelete = testContext.Users.Count();

            // Assert
            Assert.Equal(expectedCountBeforeDelete, actualCountBeforeDelete);
            Assert.Equal(expectedCountAfterDelete, actualCountAfterDelete);
        }

        [Fact]
        public async Task DeleteByObject_When_NewObjectObject_Then_Exception()
        {
            // Arrange
            UserRepository repository = new UserRepository(testContext);

            User entityToDelete = new User { Id = 10, FirstName = "Name 10", LastName = "LastName", Email = "email@mail.com", Birthday = Date.Now, RegisteredAt = Date.Now.AddMonths(5) };
            int expectedCountBeforeDelete = TestData.TotalEntitiesCount;
            int expectedCountAfterDelete = TestData.TotalEntitiesCount;

            // Act
            int actualCountBeforeDelete = testContext.Users.Count();
            await repository.DeleteAsync(entityToDelete);
            int actualCountAfterDelete = testContext.Users.Count();

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
            UserRepository repository = new UserRepository(testContext);

            int expectedEntitiesBeforeInsertCount = TestData.TotalEntitiesCount;
            int expectedEntitiesAfterInsertCount = TestData.TotalEntitiesCount + 1;

            User entityToInsert = new User { Id = 10, FirstName = "Name 10", LastName = "LastName", Email = "email@mail.com", Birthday = Date.Now, RegisteredAt = Date.Now.AddMonths(5) };

            // Act
            int actualEntitiesBeforeInsertCount = testContext.Users.Count();
            await repository.InsertAsync(entityToInsert);
            testContext.SaveChanges();
            int actualEntitiesAfterInsertCount = testContext.Users.Count();

            // Assert
            Assert.Equal(expectedEntitiesBeforeInsertCount, actualEntitiesBeforeInsertCount);
            Assert.Equal(expectedEntitiesAfterInsertCount, actualEntitiesAfterInsertCount);
            Assert.Contains(entityToInsert, testContext.Users);
        }
        [Fact]
        public async Task Insert_When_AlreadyExistedData_Then_Insert()
        {
            // Arrange
            UserRepository repository = new UserRepository(testContext);

            int expectedEntitiesBeforeInsertCount = TestData.TotalEntitiesCount;
            int expectedEntitiesAfterInsertCount = TestData.TotalEntitiesCount;

            User entityToInsert = testContext.Users.First();

            // Act
            int actualEntitiesBeforeInsertCount = testContext.Users.Count();
            await repository.InsertAsync(entityToInsert);
            Assert.Throws<ArgumentException>(() => testContext.SaveChanges());
            int actualEntitiesAfterInsertCount = testContext.Users.Count();

            // Assert
            Assert.Equal(expectedEntitiesBeforeInsertCount, actualEntitiesBeforeInsertCount);
            Assert.Equal(expectedEntitiesAfterInsertCount, actualEntitiesAfterInsertCount);
            Assert.Contains(entityToInsert, testContext.Users);
        }
        #endregion
    }
}
