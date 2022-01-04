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
    public class TeamRepositoryTest : IDisposable
    {
        // FIELDS
        BinaryDbContext testContext;
        TeamRepositoryFakeDataInitializer testData;

        // CONSTRUCTORS
        public TeamRepositoryTest()
        {
            testData = new TeamRepositoryFakeDataInitializer();
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
        TeamRepositoryFakeDataInitializer TestData => testData;

        // TESTS
        #region COUNT
        [Fact]
        public async Task CountTest()
        {
            // Arrange
            TeamRepository repository = new TeamRepository(testContext);

            int expectedEntitiesCount = TestData.TotalEntitiesCount;
            int expectedWrongCount = 0;

            // Act
            int actualEntitiesCount = await repository.CountAsync();
            int actualWrongCount = await repository.CountAsync(p => p.Name.Contains("Wrong"));

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
            TeamRepository repository = new TeamRepository(testContext);

            Team expectedEntity = testContext.Teams.First();
            int entityId = expectedEntity.Id;

            int wrongId = -1;

            // Act
            Team actualEntity = await repository.GetAsync(entityId);
            Team actualNullResult = await repository.GetAsync(wrongId);

            // Assert
            Assert.Equal(expectedEntity, actualEntity);
            Assert.Null(actualNullResult);
        }
        [Fact]
        public async Task GetByPredicate_When_CorrectPredicate_Then_GetCorrectResult()
        {
            // Arrange
            TeamRepository repository = new TeamRepository(testContext);

            int expectedEntityCount = TestData.TotalEntitiesCount;
            Expression<Func<Team, bool>> predicateToGet = p => true;
            IEnumerable<Team> expectedEntities = testContext.Teams.Where(predicateToGet);

            // Act
            IEnumerable<Team> actualEntities = await repository.GetAsync(predicateToGet);
            int actualEntitiesCount = actualEntities.Count();

            // Assert
            Assert.Equal(expectedEntities, actualEntities);
            Assert.Equal(expectedEntityCount, actualEntitiesCount);
        }

        [Fact]
        public async Task GetByPredicate_When_NullPredicate_Then_GetAllData()
        {
            // Arrange
            TeamRepository repository = new TeamRepository(testContext);

            int expectedEntityCount = TestData.TotalEntitiesCount;
            Expression<Func<Team, bool>> predicateToGet = null;
            IEnumerable<Team> expectedEntities = testContext.Teams;

            // Act
            IEnumerable<Team> actualEntities = await repository.GetAsync(predicateToGet);
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
            TeamRepository repository = new TeamRepository(testContext);

            Team entityToDelete = testContext.Teams.First();
            int idToDelete = entityToDelete.Id;
            int expectedCountBeforeDelete = TestData.TotalEntitiesCount;
            int expectedCountAfterDelete = TestData.TotalEntitiesCount - 1;

            // Act
            int actualCountBeforeDelete = testContext.Teams.Count();
            await repository.DeleteAsync(idToDelete);
            testContext.SaveChanges();
            int actualCountAfterDelete = testContext.Teams.Count();

            // Assert
            Assert.Equal(expectedCountBeforeDelete, actualCountBeforeDelete);
            Assert.Equal(expectedCountAfterDelete, actualCountAfterDelete);
            Assert.DoesNotContain(testContext.Users, x => x.Id == idToDelete);
            Assert.DoesNotContain(entityToDelete, testContext.Teams);
        }
        [Fact]
        public async Task DeleteById_When_WrongId_Then_DoNotDelete()
        {
            // Arrange
            TeamRepository repository = new TeamRepository(testContext);

            int idToDelete = -1;
            int expectedCountBeforeDelete = TestData.TotalEntitiesCount;
            int expectedCountAfterDelete = TestData.TotalEntitiesCount;

            // Act
            int actualCountBeforeDelete = testContext.Teams.Count();
            await repository.DeleteAsync(idToDelete);
            testContext.SaveChanges();
            int actualCountAfterDelete = testContext.Teams.Count();

            // Assert
            Assert.Equal(expectedCountBeforeDelete, actualCountBeforeDelete);
            Assert.Equal(expectedCountAfterDelete, actualCountAfterDelete);
        }
        [Fact]
        public async Task DeleteByObject_When_CorrectObject_Then_Delete()
        {
            // Arrange
            TeamRepository repository = new TeamRepository(testContext);

            Team entityToDelete = testContext.Teams.First();
            int expectedCountBeforeDelete = TestData.TotalEntitiesCount;
            int expectedCountAfterDelete = TestData.TotalEntitiesCount - 1;

            // Act
            int actualCountBeforeDelete = testContext.Teams.Count();
            await repository.DeleteAsync(entityToDelete);
            testContext.SaveChanges();
            int actualCountAfterDelete = testContext.Teams.Count();

            // Assert
            Assert.Equal(expectedCountBeforeDelete, actualCountBeforeDelete);
            Assert.Equal(expectedCountAfterDelete, actualCountAfterDelete);
            Assert.DoesNotContain(entityToDelete, testContext.Teams);
        }

        [Fact]
        public async Task DeleteByObject_When_NullObject_Then_DoNotDelete()
        {
            // Arrange
            TeamRepository repository = new TeamRepository(testContext);

            Team entityToDelete = null;
            int expectedCountBeforeDelete = TestData.TotalEntitiesCount;
            int expectedCountAfterDelete = TestData.TotalEntitiesCount;

            // Act
            int actualCountBeforeDelete = testContext.Teams.Count();
            await repository.DeleteAsync(entityToDelete);
            testContext.SaveChanges();
            int actualCountAfterDelete = testContext.Teams.Count();

            // Assert
            Assert.Equal(expectedCountBeforeDelete, actualCountBeforeDelete);
            Assert.Equal(expectedCountAfterDelete, actualCountAfterDelete);
        }

        [Fact]
        public async Task DeleteByObject_When_NewObjectObject_Then_Exception()
        {
            // Arrange
            TeamRepository repository = new TeamRepository(testContext);

            Team entityToDelete = new Team { Id = 10, Name = "Name 10", CreatedAt = Date.Now };
            int expectedCountBeforeDelete = TestData.TotalEntitiesCount;
            int expectedCountAfterDelete = TestData.TotalEntitiesCount;

            // Act
            int actualCountBeforeDelete = testContext.Teams.Count();
            await repository.DeleteAsync(entityToDelete);
            int actualCountAfterDelete = testContext.Teams.Count();

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
            TeamRepository repository = new TeamRepository(testContext);

            int expectedEntitiesBeforeInsertCount = TestData.TotalEntitiesCount;
            int expectedEntitiesAfterInsertCount = TestData.TotalEntitiesCount + 1;

            Team entityToInsert = new Team { Id = 10, Name = "Name 10", CreatedAt = Date.Now };

            // Act
            int actualEntitiesBeforeInsertCount = testContext.Teams.Count();
            await repository.InsertAsync(entityToInsert);
            testContext.SaveChanges();
            int actualEntitiesAfterInsertCount = testContext.Teams.Count();

            // Assert
            Assert.Equal(expectedEntitiesBeforeInsertCount, actualEntitiesBeforeInsertCount);
            Assert.Equal(expectedEntitiesAfterInsertCount, actualEntitiesAfterInsertCount);
            Assert.Contains(entityToInsert, testContext.Teams);
        }

        [Fact]
        public async Task Insert_When_AlreadyExistedData_Then_Insert()
        {
            // Arrange
            TeamRepository repository = new TeamRepository(testContext);

            int expectedEntitiesBeforeInsertCount = TestData.TotalEntitiesCount;
            int expectedEntitiesAfterInsertCount = TestData.TotalEntitiesCount;

            Team entityToInsert = testContext.Teams.First();

            // Act
            int actualEntitiesBeforeInsertCount = testContext.Teams.Count();
            await repository.InsertAsync(entityToInsert);
            Assert.Throws<ArgumentException>(() => testContext.SaveChanges());
            int actualEntitiesAfterInsertCount = testContext.Teams.Count();

            // Assert
            Assert.Equal(expectedEntitiesBeforeInsertCount, actualEntitiesBeforeInsertCount);
            Assert.Equal(expectedEntitiesAfterInsertCount, actualEntitiesAfterInsertCount);
            Assert.Contains(entityToInsert, testContext.Teams);
        }
        #endregion

        #region ADD_USER_TO_TEAM
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task AddUserToTeam_When_CorrectData_Then_Insert(int teamIdData)
        {
            // Arrange
            TeamRepository repository = new TeamRepository(testContext);
            UserRepository userRepository = new UserRepository(testContext);

            int teamId = teamIdData;

            int userId = 1;
            User user = new User { Id = userId, FirstName = "Name", LastName = "LastName", Email = "email@mail.com", Birthday = Date.Now, RegisteredAt = Date.Now.AddMonths(5), TeamId = teamId };

            User[] usersInTeamBeforeInsert = testContext.Teams.Find(teamId).Users.ToArray();

            // Act
            await userRepository.InsertAsync(user);
            testContext.SaveChanges();

            User addedToTeamUser = testContext.Users.Find(userId);
            Team teamWithUser = testContext.Teams.Find(teamId);

            // Assert
            Assert.Empty(usersInTeamBeforeInsert);
            Assert.NotEmpty(teamWithUser.Users);
            Assert.Contains(user, teamWithUser.Users);
            Assert.NotNull(user.Team);
            Assert.Equal(user.Team, teamWithUser);
        }
        #endregion
    }
}
