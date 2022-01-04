using AutoMapper;

using BusinessLayer.Commands;
using BusinessLayer.Interfaces;
using BusinessLayer.Queries;

using DataAccessLayer.Context;
using DataAccessLayer.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Server.IntegratedTests.TestData;

using System;

namespace Server.IntegratedTests.Helpers
{
    public class ControllerTestBase : IDisposable
    {
        // FIELDS
        protected FakeDataInitializer dataInitializer;

        protected Moq.Mock<IServiceProvider> serviceProviderMock;

        protected BinaryDbContext binaryDbContext;

        protected IUnitOfWork unitOfWork;

        protected IQueryProcessor queryProcessor;
        protected ICommandProcessor commandProcessor;

        // CONSTRUCTORS
        public ControllerTestBase()
        {
            dataInitializer = new FakeDataInitializer();

            serviceProviderMock = new Moq.Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(x => x.GetService(typeof(IMapper)))
                .Returns(BuildConfiguration().CreateMapper());

            binaryDbContext = new BinaryDbContext(CreateNewContextOptions(), null);

            binaryDbContext.ResetValueGenerators();
            binaryDbContext.Database.EnsureDeleted();
            dataInitializer.Seed(binaryDbContext);

            unitOfWork = new UnitOfWork(binaryDbContext);

            queryProcessor = new QueryProcessor(unitOfWork);
            commandProcessor = new CommandProcessor(unitOfWork);
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
                .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll)
                .UseInternalServiceProvider(serviceProvider)
                .Options;
        }
        public MapperConfiguration BuildConfiguration()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Core.DataTransferObjects.Task.CreateTaskDTO, DataAccessLayer.Entities.Task>();
                cfg.CreateMap<Core.DataTransferObjects.Team.CreateTeamDTO, DataAccessLayer.Entities.Team>();
                cfg.CreateMap<Core.DataTransferObjects.User.CreateUserDTO, DataAccessLayer.Entities.User>();
                cfg.CreateMap<Core.DataTransferObjects.Project.CreateProjectDTO, DataAccessLayer.Entities.Project>();
            });
        }
        public void Dispose()
        {
            binaryDbContext.Dispose();
            unitOfWork.Dispose();
        }

        // PROPERTIES
        public ControllerContext GetDefaultControllerContext => new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
            {
                RequestServices = serviceProviderMock.Object
            }
        };
    }
}
