using Xunit;

using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;

using Microsoft.EntityFrameworkCore;

using Moq;

namespace DataAccessLayer.Test.Context
{
    public class UnitOfWorkTest
    {
        [Fact]
        public void GetRepository_When_TheSameRepo_Then_LazyLoadedRepo()
        {
            // Arrange
            Mock<DbContext> mockContext = new Mock<DbContext>();
            
            UnitOfWork unitOfWork = new UnitOfWork(mockContext.Object);

            // Act
            RepositoryBase<Project> firstProjectRepos = unitOfWork.GetRepository<Project, RepositoryBase<Project>>();
            RepositoryBase<Project> secondProjectRepos = unitOfWork.GetRepository<Project, RepositoryBase<Project>>();
            RepositoryBase<Team> teamRepos = unitOfWork.GetRepository<Team, RepositoryBase<Team>>();

            // Assert
            Assert.NotNull(firstProjectRepos);
            Assert.NotNull(secondProjectRepos);
            Assert.NotNull(teamRepos);

            Assert.Same(firstProjectRepos, secondProjectRepos);
            Assert.NotSame(firstProjectRepos, teamRepos);
        }
        [Fact]
        public void Dispose_Should_DisposeOnce_When_CallDispose()
        {
            // Arrange
            Mock<DbContext> mockContext = new Mock<DbContext>();

            UnitOfWork unitOfWork = new UnitOfWork(mockContext.Object);

            // Act
            unitOfWork.Dispose();

            // Assert
            mockContext.Verify(c => c.Dispose(), Times.Once);
        }
        [Fact]
        public async System.Threading.Tasks.Task Save_Should_SaveOnce_When_CallSave()
        {
            // Arrange
            Mock<DbContext> mockContext = new Mock<DbContext>();

            UnitOfWork unitOfWork = new UnitOfWork(mockContext.Object);

            // Act
            await unitOfWork.SaveAsync();

            // Assert
            mockContext.Verify(c => c.SaveChangesAsync(default(System.Threading.CancellationToken)), Times.Once);
        }
    }
}