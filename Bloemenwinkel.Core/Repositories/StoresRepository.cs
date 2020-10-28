using System.Collections.Generic;
using System.Linq;
using Architecture_3IMD.Data;
using Architecture_3IMD.Models;
using Architecture_3IMD.Models.Domain;
using Microsoft.EntityFrameworkCore;

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

        public IEnumerable<Stores> GetAllStores()
        {
            return _context.Stores.ToList();
        }

        public Stores GetOneStoreById(int Id)
        {
            return _context.Stores.Find(Id);
        }

        public void Delete(int Id)
        {
            var store = _context.Stores.Find(Id);
            if (store == null)
            {
               //throw new NotFoundException();
               
            }

            _context.Stores.Remove(store);
            _context.SaveChanges();
        }

        public Stores Insert(int Id, string Name, string Address, string Region)
        {
            //CheckBouquetExists(Id);
            var store = new Stores
            {
               Id = Id,
               Name = Name,
               Address = Address,
               Region = Region
            };
            _context.Stores.Add(store);
            _context.SaveChanges();
            return store;
        }

        public Stores Update(int Id, string Name, string Address, string Region)
        {
            var store = _context.Stores.Find(Id);
            if (store == null)
            {
               //throw  new NotFoundException();
               
            }

            store.Name = Name;
            store.Address = Address;
            store.Region = Region;
            _context.SaveChanges();
            return store;
        }
    }
}