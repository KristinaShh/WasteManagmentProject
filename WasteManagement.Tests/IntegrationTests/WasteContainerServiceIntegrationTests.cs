using FizzWare.NBuilder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WasteContainer.Services;
using WasteManagement.Data.DatabaseContext;
using WasteManagement.Data.Repositories;
using WasteManagement.Models.Interfaces;
using WasteManagement.Models.Models;

namespace WasteManagement.Tests.IntegrationTests
{
    [TestFixture()]
    public class WasteContainerServiceIntegrationTests
    {
        private IWasteContainerRepository _wasteContainerRepository;
        private IWasteContainerService _wasteContainerService;
        private IWasteManagementDbContext _memoryDbContext;
        private string SearchNameString = "gjorche";
        private IList<WasteContainerDtoModel> _container = new List<WasteContainerDtoModel>();


        [SetUp]
        public void Init()
        {
            var services = new ServiceCollection();

            var options = new DbContextOptionsBuilder<WasteManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .Options;
            _memoryDbContext = new WasteManagementDbContext(options);

            //Arrange
            var _loggerRepo = Substitute.For<ILogger<WasteContainerService>>();

            services.AddTransient<IWasteContainerRepository, WasteContainerRepository>();
            services.AddTransient<IWasteContainerService, WasteContainerService>();

            var serviceProvider = services.BuildServiceProvider();


            _wasteContainerRepository = new WasteContainerRepository(_memoryDbContext);
            _wasteContainerService = new WasteContainerService(_wasteContainerRepository, _loggerRepo);

            _container = Builder<WasteContainerDtoModel>.CreateListOfSize(10).TheFirst(5)
                    .With(p => p.Name = SearchNameString)
                    .All()
                    .With(c => c.Id = Guid.NewGuid()).Build();

            if (_memoryDbContext.Containers.Count() == 0)
            {
                _memoryDbContext.Containers.AddRange(_container);
                _memoryDbContext.SaveChanges(true);
            }

        }


        [Test()]
        public void AddTest()
        {
            var count = _memoryDbContext.Containers.Count();
            var wasteManagementCreateModel = Builder<WasteContainerCreateModel>.CreateNew().Build();

            Assert.IsTrue(_wasteContainerService.Add(wasteManagementCreateModel));
            Assert.IsTrue(_memoryDbContext.Containers.Count() == count + 1);
        }

        [Test()]
        public void DeleteTest()
        {
            int count = _memoryDbContext.Containers.Count();
            var entityId = _memoryDbContext.Containers.FirstOrDefault();

            Assert.IsTrue(_wasteContainerService.Delete(entityId.Id));
            Assert.IsTrue(_memoryDbContext.Containers.Count() == count - 1);
        }

        [Test()]
        public void ExistsTest()
        {
            var entities = _memoryDbContext.Containers.FirstOrDefault();

            Assert.IsTrue(_wasteContainerService.Exists(entities.Id));
        }

        [Test()]
        public void GetWasteContainersTest()
        {
            int count = _memoryDbContext.Containers.Count();
            Assert.IsTrue(count == _wasteContainerService.GetWasteContainers().Count);
            Assert.IsTrue(_wasteContainerService.GetWasteContainers("name", SearchNameString).Count > 0);
        }


        [Test()]
        public void GetWasteContainerAsEditModelTest()
        {
            var entity = _memoryDbContext.Containers.FirstOrDefault();
            var entityEditModel = _wasteContainerService.GetWasteContainerAsEditModel(entity.Id);

            Assert.IsTrue(entity.Name == entityEditModel.Name);
            Assert.IsTrue(entity.Location == entityEditModel.Location);
            Assert.IsTrue(entity.Id == entityEditModel.Id);
        }

        [Test()]
        public void GetWasteContainerAsViewModelTest()
        {
            var entity = _memoryDbContext.Containers.FirstOrDefault();
            var entityEditModel = _wasteContainerService.GetWasteContainerAsViewModel(entity.Id);

            Assert.IsTrue(entity.Name == entityEditModel.Name);
            Assert.IsTrue(entity.Location == entityEditModel.Location);
            Assert.IsTrue(entity.Id == entityEditModel.Id);
        }


        [Test()]
        public void UpdateTest()
        {
            var entity = _memoryDbContext.Containers.FirstOrDefault();
            var entityEditModel = Builder<WasteContainerUpdateModel>.CreateNew()
                .With(c => c.Id = entity.Id).Build();

            Assert.IsTrue(_wasteContainerService.Update(entityEditModel));
        }
    }

}