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

namespace DataAccessLayer.Test.Repositories
{
    public class ProjectRepositoryTest : IDisposable
    {
        // FIELDS
        BinaryDbContext testContext;
        ProjectRepositoryFakeDataInitializer testData;

        // CONSTRUCTORS
        public ProjectRepositoryTest()
        {
            testData = new ProjectRepositoryFakeDataInitializer();
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
        ProjectRepositoryFakeDataInitializer TestData => testData;

        // TESTS
        #region COUNT
        [Fact]
        public async Task CountTest()
        {
            // Arrange
            ProjectRepository projectRepository = new ProjectRepository(testContext);

            int expectedProjectCount         = TestData.ProjectsAmount;
            int expectedCalledProjectCount   = TestData.CalledProjectAmount;
            int expectedCalledNamedCount     = TestData.CalledNameAmount;
            int expectedWrongCount           = 0;

            // Act
            int actualProjectCount          = await projectRepository.CountAsync();
            int actualCalledProjectCount    = await projectRepository.CountAsync(p => p.Name.Contains("Project"));
            int actualCalledNamedCount      = await projectRepository.CountAsync(p => p.Name.Contains("Name"));
            int actualWrongCount            = await projectRepository.CountAsync(p => p.Name.Contains("Wrong"));

            // Assert
            Assert.Equal(expectedProjectCount,         actualProjectCount);
            Assert.Equal(expectedCalledProjectCount,   actualCalledProjectCount);
            Assert.Equal(expectedCalledNamedCount,     actualCalledNamedCount);
            Assert.Equal(expectedWrongCount,           actualWrongCount);

            await Assert.ThrowsAsync<ArgumentNullException>(() => projectRepository.CountAsync(null));
        }
        #endregion

        #region GET
        [Fact]
        public async Task GetById()
        {
            // Arrange
            ProjectRepository projectRepository = new ProjectRepository(testContext);

            Project expectedEntity = testContext.Projects.First();
            int entityId = expectedEntity.Id;

            int wrongId = -1;

            // Act
            Project actualEntity = await projectRepository.GetAsync(entityId);
            Project actualNullResult = await projectRepository.GetAsync(wrongId);

            // Assert
            Assert.Equal(expectedEntity, actualEntity);
            Assert.Null(actualNullResult);
        }
        [Fact]
        public async Task GetByPredicate_When_CorrectPredicate_Then_GetCorrectResult()
        {
            // Arrange
            ProjectRepository repository = new ProjectRepository(testContext);

            int expectedEntityCount = TestData.CalledProjectAmount;
            Expression<Func<Project, bool>> predicateToGet = p => p.Name.Contains("Project");
            IEnumerable<Project> expectedEntities = testContext.Projects.Where(predicateToGet);

            // Act
            IEnumerable<Project> actualEntities = await repository.GetAsync(predicateToGet);
            int actualEntitiesCount = actualEntities.Count();

            // Assert
            Assert.Equal(expectedEntities, actualEntities);
            Assert.Equal(expectedEntityCount, actualEntitiesCount);
        }

        [Fact]
        public async Task GetByPredicate_When_NullPredicate_Then_GetAllData()
        {
            // Arrange
            ProjectRepository repository = new ProjectRepository(testContext);

            int expectedEntityCount = TestData.ProjectsAmount;
            Expression<Func<Project, bool>> predicateToGet = null;
            IEnumerable<Project> expectedEntities = testContext.Projects;

            // Act
            IEnumerable<Project> actualEntities = await repository.GetAsync(predicateToGet);
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
            ProjectRepository repository = new ProjectRepository(testContext);

            Project entityToDelete = testContext.Projects.First();
            int idToDelete = entityToDelete.Id;
            int expectedCountBeforeDelete = TestData.ProjectsAmount;
            int expectedCountAfterDelete = TestData.ProjectsAmount - 1;

            // Act
            int actualCountBeforeDelete = testContext.Projects.Count();
            await repository.DeleteAsync(idToDelete);
            testContext.SaveChanges();
            int actualCountAfterDelete = testContext.Projects.Count();

            // Assert
            Assert.Equal(expectedCountBeforeDelete, actualCountBeforeDelete);
            Assert.Equal(expectedCountAfterDelete, actualCountAfterDelete);
            Assert.DoesNotContain(testContext.Projects, x => x.Id == idToDelete);
            Assert.DoesNotContain(entityToDelete, testContext.Projects);
        }
        [Fact]
        public async Task DeleteById_When_WrongId_Then_DoNotDelete()
        {
            // Arrange
            ProjectRepository repository = new ProjectRepository(testContext);
            
            int idToDelete = -1;
            int expectedCountBeforeDelete = TestData.ProjectsAmount;
            int expectedCountAfterDelete = TestData.ProjectsAmount;

            // Act
            int actualCountBeforeDelete = testContext.Projects.Count();
            await repository.DeleteAsync(idToDelete);
            testContext.SaveChanges();
            int actualCountAfterDelete = testContext.Projects.Count();

            // Assert
            Assert.Equal(expectedCountBeforeDelete, actualCountBeforeDelete);
            Assert.Equal(expectedCountAfterDelete, actualCountAfterDelete);
        }
        [Fact]
        public async Task DeleteByObject_When_CorrectObject_Then_Delete()
        {
            // Arrange
            ProjectRepository repository = new ProjectRepository(testContext);

            Project entityToDelete = testContext.Projects.First();
            int expectedCountBeforeDelete = TestData.ProjectsAmount;
            int expectedCountAfterDelete = TestData.ProjectsAmount - 1;

            // Act
            int actualCountBeforeDelete = testContext.Projects.Count();
            await repository.DeleteAsync(entityToDelete);
            testContext.SaveChanges();
            int actualCountAfterDelete = testContext.Projects.Count();

            // Assert
            Assert.Equal(expectedCountBeforeDelete, actualCountBeforeDelete);
            Assert.Equal(expectedCountAfterDelete, actualCountAfterDelete);
            Assert.DoesNotContain(entityToDelete, testContext.Projects);
        }

        [Fact]
        public async Task DeleteByObject_When_NullObject_Then_DoNotDelete()
        {
            // Arrange
            ProjectRepository repository = new ProjectRepository(testContext);

            Project entityToDelete = null;
            int expectedCountBeforeDelete = TestData.ProjectsAmount;
            int expectedCountAfterDelete = TestData.ProjectsAmount;

            // Act
            int actualCountBeforeDelete = testContext.Projects.Count();
            await repository.DeleteAsync(entityToDelete);
            testContext.SaveChanges();
            int actualCountAfterDelete = testContext.Projects.Count();

            // Assert
            Assert.Equal(expectedCountBeforeDelete, actualCountBeforeDelete);
            Assert.Equal(expectedCountAfterDelete, actualCountAfterDelete);
        }

        [Fact]
        public async Task DeleteByObject_When_NewObjectObject_Then_Exception()
        {
            // Arrange
            ProjectRepository repository = new ProjectRepository(testContext);

            Project entityToDelete = new Project { Id = 10, Name = "Project 10", CreatedAt = DateTime.Now, Deadline = DateTime.Now.AddMonths(5), Description = "Description" }; ;
            int expectedCountBeforeDelete = TestData.ProjectsAmount;
            int expectedCountAfterDelete = TestData.ProjectsAmount;

            // Act
            int actualCountBeforeDelete = testContext.Projects.Count();
            await repository.DeleteAsync(entityToDelete);
            int actualCountAfterDelete = testContext.Projects.Count();

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
            ProjectRepository repository = new ProjectRepository(testContext);

            int expectedEntitiesBeforeInsertCount = TestData.ProjectsAmount;
            int expectedEntitiesAfterInsertCount = TestData.ProjectsAmount + 1;

            Project entityToInsert = new Project { Id = 10, Name = "Project 10", CreatedAt = DateTime.Now, Deadline = DateTime.Now.AddMonths(5), Description = "Description" }; ;

            // Act
            int actualEntitiesBeforeInsertCount = testContext.Projects.Count();
            await repository.InsertAsync(entityToInsert);
            testContext.SaveChanges();
            int actualEntitiesAfterInsertCount = testContext.Projects.Count();

            // Assert
            Assert.Equal(expectedEntitiesBeforeInsertCount, actualEntitiesBeforeInsertCount);
            Assert.Equal(expectedEntitiesAfterInsertCount, actualEntitiesAfterInsertCount);
            Assert.Contains(entityToInsert, testContext.Projects);
        }

        [Fact]
        public async Task Insert_When_AlreadyExistedData_Then_Insert()
        {
            // Arrange
            ProjectRepository repository = new ProjectRepository(testContext);

            int expectedEntitiesBeforeInsertCount = TestData.ProjectsAmount;
            int expectedEntitiesAfterInsertCount = TestData.ProjectsAmount;

            Project entityToInsert = testContext.Projects.First();

            // Act
            int actualEntitiesBeforeInsertCount = testContext.Projects.Count();
            await repository.InsertAsync(entityToInsert);
            Assert.Throws<ArgumentException>(() => testContext.SaveChanges());
            int actualEntitiesAfterInsertCount = testContext.Projects.Count();

            // Assert
            Assert.Equal(expectedEntitiesBeforeInsertCount, actualEntitiesBeforeInsertCount);
            Assert.Equal(expectedEntitiesAfterInsertCount, actualEntitiesAfterInsertCount);
            Assert.Contains(entityToInsert, testContext.Projects);
        }
        #endregion
    }
}
