/*using System;
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
    public class SaleControllerTests : IDisposable
    {
        private readonly Mock<ILogger<SaleController>> _loggerMock;
        private readonly Mock<ISalesRepository> _saleRepoMock;
        private readonly SaleController _saleController;

        public SaleControllerTests()
        {
            _loggerMock = new Mock<ILogger<SaleController>>(MockBehavior.Loose);
            _saleRepoMock = new Mock<ISalesRepository>(MockBehavior.Strict);
            _saleController = new SaleController(_saleRepoMock.Object, _loggerMock.Object);
        }
        public void Dispose()
        {
            _loggerMock.VerifyAll();
            _saleRepoMock.VerifyAll();

            _loggerMock.Reset();
            _saleRepoMock.Reset();
        }


        [Fact]
        public async Task TestGetAllSales()
        {
            var returnSet = new[]
            {
                new Sale
                {
                    Id = 1,
                    Store_id = 1,
                    Bouquet_id = 1,
                    Amount = 1,
                    FirstName = "test",
                    Lastname = "test"
                },
                new Sale
                {
                    Id = 2,
                    Store_id = 2,
                    Bouquet_id = 2,
                    Amount = 2,
                    FirstName = "test",
                    Lastname = "test"
                },
                new Sale
                {
                    Id = 3,
                    Store_id = 3,
                    Bouquet_id = 3,
                    Amount = 3,
                    FirstName = "test",
                    Lastname = "test"
                },
            };
            // Arrange
            _saleRepoMock.Setup(x => x.GetSales()).ReturnsAsync(returnSet).Verifiable();

            // Act
            var saleResponse = await _saleController.getAllSales();

            // Assert
            saleResponse.Should().BeOfType<OkObjectResult>();

            // verify via a snapshot (https://swisslife-oss.github.io/snapshooter/)
            // used a lot in jest (for JS)
            Snapshot.Match(saleResponse);
        }

        [Fact]
        public async Task TestCreateSale()
        {
            var sale = new Sale()
            {
                Id = 123,
                Store_id = 123,
                Bouquet_id = 123,
                Amount = 123,
                FirstName = "test",
                Lastname = "test"
            };            
            _saleRepoMock.Setup(x => x.Insert(123, 123, 123, 123, "test", "test")).ReturnsAsync(sale).Verifiable();
            var saleResponse = await _saleController.createSale(new SaleUpsertInput()
            {
                Id = 123,
                Store_id = 123,
                Bouquet_id = 123,
                Amount = 123,
                FirstName = "test",
                Lastname = "test"
            });
            saleResponse.Should().BeOfType<CreatedResult>();
            Snapshot.Match(saleResponse);
        }

        [Fact]
        public async Task TestAddSale()
        {
            var sale = new Sale()
            {
                Id = 123,
                Store_id = 456,
                Bouquet_id = 456,
                Amount = 456
            };            
            _saleRepoMock.Setup(x => x.Update(123, 456, 456, 456)).ReturnsAsync(sale).Verifiable();
            var saleResponse = await _saleController.addSale(new SaleUpsertInput()
            {
                Id = 123,
                Store_id = 456,
                Bouquet_id = 456,
                Amount = 456
            });
            saleResponse.Should().BeOfType<AcceptedResult>();
            Snapshot.Match(saleResponse);
        }
    }
}
*/
