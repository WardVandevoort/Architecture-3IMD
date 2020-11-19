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
    public class SaleTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public SaleTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetSalesEndPointReturnsNoDataWhenDbIsEmpty()
        {
            var client = _factory.CreateClient();
            _factory.ResetAndSeedDatabase((db) => { });
            var response = await client.GetAsync("/Flowershop/Sale");
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Snapshot.Match(await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task GetSalesEndPointReturnsSomeDataWhenDbIsNotEmpty()
        {
            var client = _factory.CreateClient();
            _factory.ResetAndSeedDatabase((db) =>
            {
                db.Sales.Add(new Sale() 
                    {
                    Id = 1,
                    Store_id = 1,
                    Bouquet_id = 1,
                    Amount = 1
                    });
                db.Sales.Add(new Sale() 
                    {
                    Id = 2,
                    Store_id = 2,
                    Bouquet_id = 2,
                    Amount = 2
                    });
            });
            var response = await client.GetAsync("/Flowershop/Sale");
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Snapshot.Match(await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task CreateSaleCombinationReturnsCorrectData()
        {
            var client = _factory.CreateClient();
            _factory.ResetAndSeedDatabase((db) => { });

            var request = new
            {
                Body = new SaleUpsertInput
                {
                    Id = 3,
                    Store_id = 123,
                    Bouquet_id = 123,
                    Amount = 123
                }
            };
            var createResponse = await client.PostAsync("/Flowershop/Sale", ContentHelper.GetStringContent(request.Body));
            createResponse.EnsureSuccessStatusCode();
            var body = JsonConvert.DeserializeObject<SaleWebOutput>(await createResponse.Content.ReadAsStringAsync());
            body.Should().NotBeNull();
            body.Store_id.Should().Be(123);
            body.Bouquet_id.Should().Be(123);
            body.Amount.Should().Be(123);
            var getResponse = await client.GetAsync($"/Flowershop/Sale/{body.Id}");
            getResponse.EnsureSuccessStatusCode();
            Snapshot.Match(await getResponse.Content.ReadAsStringAsync());
        }

    }
}
