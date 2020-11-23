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
    public class StoreTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public StoreTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetStoresEndPointReturnsNoDataWhenDbIsEmpty()
        {
            var client = _factory.CreateClient();
            _factory.ResetAndSeedDatabase((db) => { });
            var response = await client.GetAsync("/Flowershop/Store");
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Snapshot.Match(await response.Content.ReadAsStringAsync(), matchOptions => matchOptions.IgnoreField("Id"));
        }

        [Fact]
        public async Task GetStoresEndPointReturnsSomeDataWhenDbIsNotEmpty()
        {
            var client = _factory.CreateClient();
            _factory.ResetAndSeedDatabase((db) =>
            {
                db.Stores.Add(new Stores() 
                    {
                    Id = 1,
                    Name = "test name 1",
                    Address = "test address 1",
                    Region = "test region 1"
                    });
                db.Stores.Add(new Stores() 
                    {
                    Id = 2,
                    Name = "test name 2",
                    Address = "test address 2",
                    Region = "test region 2"
                    });
            });
            var response = await client.GetAsync("/Flowershop/Store");
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Snapshot.Match(await response.Content.ReadAsStringAsync(), matchOptions => matchOptions.IgnoreField("Id"));
        }

        [Fact]
        public async Task CreateStoreReturnsCorrectData()
        {
            var client = _factory.CreateClient();
            _factory.ResetAndSeedDatabase((db) => { });

            var request = new
            {
                Body = new StoreUpsertInput
                {
                    Id = 3,
                    Name = "test name",
                    Address = "test address",
                    Region = "test region"
                }
            };
            var createResponse = await client.PostAsync("/Flowershop/Store", ContentHelper.GetStringContent(request.Body));
            createResponse.EnsureSuccessStatusCode();
            var body = JsonConvert.DeserializeObject<StoreWebOutput>(await createResponse.Content.ReadAsStringAsync());
            body.Should().NotBeNull();
            body.Name.Should().Be("test name");
            body.Address.Should().Be("test address");
            body.Region.Should().Be("test region");
            var getResponse = await client.GetAsync($"/Flowershop/Store/{body.Id}");
            getResponse.EnsureSuccessStatusCode();
            Snapshot.Match(await getResponse.Content.ReadAsStringAsync(), matchOptions => matchOptions.IgnoreField("Id"));
        }

        [Fact]
        public async Task CreateStoreThrowsErrorOnEmptyName()
        {
            var client = _factory.CreateClient();
            _factory.ResetAndSeedDatabase((db) => { });

            var request = new
            {
                Body = new StoreUpsertInput
                {
                    Id = 4,
                    Name = string.Empty,
                    Address = "test address",
                    Region = "test region"
                }
            };
            var createResponse = await client.PostAsync("/Flowershop/Store", ContentHelper.GetStringContent(request.Body));
            createResponse.StatusCode.Should().Be(400);
            Snapshot.Match(await createResponse.Content.ReadAsStringAsync(), matchOptions => matchOptions.IgnoreField("Id"));
        }

        [Fact]
        public async Task CreateStoreThrowsErrorOnGiganticName()
        {
            var client = _factory.CreateClient();
            _factory.ResetAndSeedDatabase((db) => { });

            var request = new
            {
                Body = new StoreUpsertInput
                {
                    Id = 5,
                    Name = new string('c', 10001),
                    Address = "test address",
                    Region = "test region"
                }
            };
            var createResponse = await client.PostAsync("/Flowershop/Store", ContentHelper.GetStringContent(request.Body));
            createResponse.StatusCode.Should().Be(400);
            Snapshot.Match(await createResponse.Content.ReadAsStringAsync(), matchOptions => matchOptions.IgnoreField("Id"));
        }

    }
}
