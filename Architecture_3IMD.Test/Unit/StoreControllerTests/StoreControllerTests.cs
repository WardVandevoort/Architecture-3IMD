using System;
using Xunit;
using System.Collections.Generic;
using Architecture_3IMD.Data;
using Architecture_3IMD.Controllers;
using Architecture_3IMD.Models;
using Architecture_3IMD.Models.Domain;
using Architecture_3IMD.Models.Web;
using Architecture_3IMD.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Snapshooter.Xunit;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using BasisRegisters.Vlaanderen;

namespace Architecture_3IMD.Test
{
    public class StoreControllerTests : IDisposable
    {
        private readonly Mock<ILogger<StoreController>> _loggerMock;
        private readonly Mock<IStoresRepository> _storeRepoMock;
        private readonly StoreController _storeController;
        private readonly IBasisRegisterService _basisRegisterService;

        public StoreControllerTests()
        {
            _loggerMock = new Mock<ILogger<StoreController>>(MockBehavior.Loose);
            _storeRepoMock = new Mock<IStoresRepository>(MockBehavior.Strict);
            _storeController = new StoreController(_storeRepoMock.Object, _loggerMock.Object, _basisRegisterService);
            
        }
        public void Dispose()
        {
            _loggerMock.VerifyAll();
            _storeRepoMock.VerifyAll();

            _loggerMock.Reset();
            _storeRepoMock.Reset();
        }


        [Fact]
        public async Task TestGetAllStores()
        {
            var returnSet = new[]
            {
                new Store
                {
                    Id = 1,
                    Name = "test name 1",
                    Address = "Zandpoortvest",
                    StreetNumber = "60",
                    Region = "Mechelen"
                },
                new Store
                {
                    Id = 2,
                    Name = "test name 2",
                    Address = "Zandpoortvest",
                    StreetNumber = "60",
                    Region = "Mechelen"
                },
                new Store
                {
                    Id = 3,
                    Name = "test name 3",
                    Address = "Zandpoortvest",
                    StreetNumber = "60",
                    Region = "Mechelen"
                },
            };
            // Arrange
            _storeRepoMock.Setup(x => x.GetAllStores()).ReturnsAsync(returnSet).Verifiable();

            // Act
            var storeResponse = await _storeController.getAllStores();

            // Assert
            storeResponse.Should().BeOfType<OkObjectResult>();

            // verify via a snapshot (https://swisslife-oss.github.io/snapshooter/)
            // used a lot in jest (for JS)
            Snapshot.Match(storeResponse);
        }

        [Fact]
        public async Task TestGetOneStore()
        {
            var store = new Store()
            {
                Id = 1,
                Name = "test name",
                Address = "Zandpoortvest",
                StreetNumber = "60",
                Region = "Mechelen"
            };
            _storeRepoMock.Setup(x => x.GetOneStoreById(1)).Returns(Task.FromResult(store)).Verifiable();
            var storeResponse = await _storeController.getOneStore(1);
            storeResponse.Should().BeOfType<OkObjectResult>();
            Snapshot.Match(storeResponse);
        }  

        [Fact]
        public async Task TestGetOneStoreNotFound()
        {
            _storeRepoMock.Setup(x => x.GetOneStoreById(1)).Returns(Task.FromResult(null as Store)).Verifiable();
            var storeResponse = await _storeController.getOneStore(1);
            storeResponse.Should().BeOfType<NotFoundResult>();
            Snapshot.Match(storeResponse);
        }

        /*[Fact]
        public async Task TestCreateStore()
        {
            var store = new Store()
            {
                Id = 1,
                Name = "test name",
                Address = "Zandpoortvest",
                StreetNumber = "60",
                Region = "Mechelen"
            };            
            _storeRepoMock.Setup(x => x.Insert(1, "test name", "Zandpoortvest", "60", "Mechelen")).ReturnsAsync(store).Verifiable();
            var storeResponse = await _storeController.createStore(new StoreUpsertInput()
            {
                Id = 1,
                Name = "test name",
                Address = "Zandpoortvest",
                StreetNumber = "60",
                Region = "Mechelen"
            });
            storeResponse.Should().BeOfType<CreatedResult>();
            Snapshot.Match(storeResponse);
        }*/
    }
}
