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
    public class BouquetsRepository : IBouquetsRepository
    {
        private readonly ApplicationDbContext _context;

        public BouquetsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Bouquet>> GetAllBouquets()
        {
            return await _context.Bouquets.ToListAsync();
        }

        public async Task<Bouquet> GetOneBouquetById(int Id)
        {
            return await _context.Bouquets.FindAsync(Id);
        }

        public async Task Delete(int Id)
        {
            var bouquet = await _context.Bouquets.FindAsync(Id);
            if (bouquet == null)
            {
               //throw new NotFoundException();
               
            }

            _context.Bouquets.Remove(bouquet);
            await _context.SaveChangesAsync();
        }

        public async Task<Bouquet> Insert(int Id, string Name, int Price, string Description)
        {
            //CheckBouquetExists(Id);
            var bouquet = new Bouquet
            {
               Id = Id,
               Name = Name,
               Price = Price,
               Description = Description
            };
            await _context.Bouquets.AddAsync(bouquet);
            await _context.SaveChangesAsync();
            return bouquet;
        }

        public async Task<Bouquet> Update(int Id, string Name, int Price, string Description)
        {
            var bouquet = await _context.Bouquets.FindAsync(Id);
            if (bouquet == null)
            {
               //throw  new NotFoundException();
               
            }

            bouquet.Name = Name;
            bouquet.Price = Price;
            bouquet.Description = Description;
            await _context.SaveChangesAsync();
            return bouquet;
        }
    }
}