using System.Threading.Tasks;
using Architecture_3IMD.Models;
using Architecture_3IMD.Models.Domain;
using Architecture_3IMD.Models.Web;
using Architecture_3IMD.Controllers;
using Architecture_3IMD.Test.Integration.Utils;
using FluentAssertions;
using Newtonsoft.Json;
using Snapshooter;
using Snapshooter.Xunit;
using Xunit;

namespace Architecture_3IMD.Test.Integration
{
    public class BouquetTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public BouquetTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetBouquetsEndPointReturnsNoDataWhenDbIsEmpty()
        {
            var client = _factory.CreateClient();
            _factory.ResetAndSeedDatabase((db) => { });
            var response = await client.GetAsync("/Flowershop/Bouquet");
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Snapshot.Match(await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task GetBouquetsEndPointReturnsSomeDataWhenDbIsNotEmpty()
        {
            var client = _factory.CreateClient();
            _factory.ResetAndSeedDatabase((db) =>
            {
                db.Bouquets.Add(new Bouquet() 
                    {
                    Id = 1,
                    Name = "test name 1",
                    Price = 123,
                    Description = "test description 1"
                    });
                db.Bouquets.Add(new Bouquet() 
                    {
                    Id = 2,
                    Name = "test name 2",
                    Price = 456,
                    Description = "test description 2"
                    });
            });
            var response = await client.GetAsync("/Flowershop/Bouquet");
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Snapshot.Match(await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task CreateBouquetReturnsCorrectData()
        {
            var client = _factory.CreateClient();
            _factory.ResetAndSeedDatabase((db) => { });

            var request = new
            {
                Body = new BouquetUpsertInput
                {
                    Id = 1,
                    Name = "test name",
                    Price = 123,
                    Description = "test description"
                }
            };
            var createResponse = await client.PostAsync("/Flowershop/Bouquet", ContentHelper.GetStringContent(request.Body));
            createResponse.EnsureSuccessStatusCode();
            var body = JsonConvert.DeserializeObject<BouquetWebOutput>(await createResponse.Content.ReadAsStringAsync());
            body.Should().NotBeNull();
            body.Name.Should().Be("test name");
            body.Price.Should().Be(123);
            body.Description.Should().Be("test description");
            var getResponse = await client.GetAsync($"/Flowershop/Bouquet/{body.Id}");
            getResponse.EnsureSuccessStatusCode();
            Snapshot.Match(await getResponse.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task CreateBouquetThrowsErrorOnEmptyName()
        {
            var client = _factory.CreateClient();
            _factory.ResetAndSeedDatabase((db) => { });

            var request = new
            {
                Body = new BouquetUpsertInput
                {
                    Id = 1,
                    Name = string.Empty,
                    Price = 123,
                    Description = "test description"
                }
            };
            var createResponse = await client.PostAsync("/Flowershop/Bouquet", ContentHelper.GetStringContent(request.Body));
            createResponse.StatusCode.Should().Be(400);
            Snapshot.Match(await createResponse.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task CreateBouquetThrowsErrorOnGiganticName()
        {
            var client = _factory.CreateClient();
            _factory.ResetAndSeedDatabase((db) => { });

            var request = new
            {
                Body = new BouquetUpsertInput
                {
                    Id = 1,
                    Name = new string('c', 10001),
                    Price = 123,
                    Description = "test description"
                }
            };
            var createResponse = await client.PostAsync("/Flowershop/Bouquet", ContentHelper.GetStringContent(request.Body));
            createResponse.StatusCode.Should().Be(400);
            Snapshot.Match(await createResponse.Content.ReadAsStringAsync());
        }

    }

}
