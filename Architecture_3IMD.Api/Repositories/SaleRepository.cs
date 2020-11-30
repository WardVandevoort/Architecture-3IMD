using System.Collections.Generic;
using System.Threading.Tasks;
using Architecture_3IMD.Data;
using Architecture_3IMD.Models;
using Architecture_3IMD.Models.Domain;
using MongoDB.Driver;
using System.Linq;
using Architecture_3IMD.Repositories;


namespace Architecture_3IMD.Repositories
{

    public class SaleRepository : ISaleRepository
    {
        public IMongoCollection<Sale> Sales { get; }
        public MongoClient MongoClient { get; }

        public SaleRepository(ISalesDbContext settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            Sales = database.GetCollection<Sale>(settings.CollectionName);
            MongoClient = client;
        }

        public async Task<List<Sale>> GetAllSalesAsync()
        {
            // you always need a filter when using mongodb
            var findCursor = await Sales.OfType<Sale>().FindAsync(x => true);
            return await findCursor.ToListAsync();
        }

        public async Task InsertSaleAsync(Sale sale)
        {
            // as soon as you insert an entity you automatically get an id filled in.
            await Sales.InsertOneAsync(sale);
        }

    }
}