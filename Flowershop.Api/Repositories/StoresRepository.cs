using System.Collections.Generic;
using System.Linq;
using Architecture_3IMD.Data;
using Architecture_3IMD.Models;
using Architecture_3IMD.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Architecture_3IMD.Repositories
{
    // Repository pattern: https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-design#the-repository-pattern
    // This class will be further expanded in later lessons, when we are talking about interfacing with databases.
    public class StoresRepository : IStoresRepository
    {
        private readonly ApplicationDbContext _context;

        public StoresRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Stores>> GetAllStores()
        {
            return await _context.Stores.ToListAsync();
        }

        public async Task<Stores>  GetOneStoreById(int Id)
        {
            return await _context.Stores.FindAsync(Id);
        }

        public async Task Delete(int Id)
        {
            var store = await _context.Stores.FindAsync(Id);
            if (store == null)
            {
               //throw new NotFoundException();
               
            }

            _context.Stores.Remove(store);
            await _context.SaveChangesAsync();
        }

        public async Task<Stores> Insert(int Id, string Name, string Address, string Region)
        {
            //CheckBouquetExists(Id);
            var store = new Stores
            {
               Id = Id,
               Name = Name,
               Address = Address,
               Region = Region
            };
            await _context.Stores.AddAsync(store);
            await _context.SaveChangesAsync();
            return store;
        }

        public async Task<Stores> Update(int Id, string Name, string Address, string Region)
        {
            var store = await _context.Stores.FindAsync(Id);
            if (store == null)
            {
               //throw  new NotFoundException();
               
            }

            store.Name = Name;
            store.Address = Address;
            store.Region = Region;
            await _context.SaveChangesAsync();
            return store;
        }
    }
}