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



namespace Architecture_3IMD.Test
{
    public class BouquetControllerTests : IDisposable
    {
        private readonly Mock<ILogger<BouquetController>> _loggerMock;
        private readonly Mock<IBouquetsRepository> _bouquetRepoMock;
        private readonly BouquetController _bouquetController;

        public BouquetControllerTests()
        {
            _loggerMock = new Mock<ILogger<BouquetController>>(MockBehavior.Loose);
            _bouquetRepoMock = new Mock<IBouquetsRepository>(MockBehavior.Strict);
            _bouquetController = new BouquetController(_bouquetRepoMock.Object, _loggerMock.Object);
        }
        public void Dispose()
        {
            _loggerMock.VerifyAll();
            _bouquetRepoMock.VerifyAll();

            _loggerMock.Reset();
            _bouquetRepoMock.Reset();
        }


        [Fact]
        public async Task TestGetAllBouquets()
        {
            var returnSet = new[]
            {
                new Bouquet
                {
                    Id = 1,
                    Name = "test name 1",
                    Price = 123,
                    Description = "test description 1"
                },
                new Bouquet
                {
                    Id = 2,
                    Name = "test name 2",
                    Price = 123,
                    Description = "test description 2"
                },
                new Bouquet
                {
                    Id = 3,
                    Name = "test name 3",
                    Price = 123,
                    Description = "test description 3"
                },
            };
            // Arrange
            _bouquetRepoMock.Setup(x => x.GetAllBouquets()).ReturnsAsync(returnSet).Verifiable();

            // Act
            var bouquetResponse = await _bouquetController.getAllBouquets();

            // Assert
            bouquetResponse.Should().BeOfType<OkObjectResult>();

            // verify via a snapshot (https://swisslife-oss.github.io/snapshooter/)
            // used a lot in jest (for JS)
            Snapshot.Match(bouquetResponse);
        }

        [Fact]
        public async Task TestCreateBouquet()
        {
            var bouquet = new Bouquet()
            {
                Id = 1,
                Name = "test name",
                Price = 10,
                Description = "test description"
            };            
            _bouquetRepoMock.Setup(x => x.Insert(1, "test name", 10, "test description")).ReturnsAsync(bouquet).Verifiable();
            var bouquetResponse = await _bouquetController.createBouquet(new BouquetUpsertInput()
            {
                Id = 1,
                Name = "test name",
                Price = 10,
                Description = "test description"
            });
            bouquetResponse.Should().BeOfType<CreatedResult>();
            Snapshot.Match(bouquetResponse);
        }
    }
}
