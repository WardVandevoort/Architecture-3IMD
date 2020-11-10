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



namespace Flowershop.Test
{
    public class StoreControllerTests : IDisposable
    {
        private readonly Mock<ILogger<StoreController>> _loggerMock;
        private readonly Mock<IStoresRepository> _storeRepoMock;
        private readonly StoreController _storeController;

        public StoreControllerTests()
        {
            _loggerMock = new Mock<ILogger<StoreController>>(MockBehavior.Loose);
            _storeRepoMock = new Mock<IStoresRepository>(MockBehavior.Strict);
            _storeController = new StoreController(_storeRepoMock.Object, _loggerMock.Object);
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
                new Stores
                {
                    Id = 1,
                    Name = "test name 1",
                    Address = "test address 1",
                    Region = "test region 1"
                },
                new Stores
                {
                    Id = 2,
                    Name = "test name 2",
                    Address = "test address 2",
                    Region = "test region 2"
                },
                new Stores
                {
                    Id = 3,
                    Name = "test name 3",
                    Address = "test address 3",
                    Region = "test region 3"
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
        public async Task TestCreateStore()
        {
            var store = new Stores()
            {
                Id = 1,
                Name = "test name",
                Address = "test address",
                Region = "test region"
            };            
            _storeRepoMock.Setup(x => x.Insert(1, "test name", "test address", "test region")).ReturnsAsync(store).Verifiable();
            var storeResponse = await _storeController.createStore(new StoreUpsertInput()
            {
                Id = 1,
                Name = "test name",
                Address = "test address",
                Region = "test region"
            });
            storeResponse.Should().BeOfType<CreatedResult>();
            Snapshot.Match(storeResponse);
        }
    }
}
