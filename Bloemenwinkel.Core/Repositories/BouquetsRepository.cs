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
    public class BouquetsRepository : IBouquetsRepository
    {
        private readonly ApplicationDbContext _context;

        public BouquetsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Bouquet> GetAllBouquets()
        {
            return _context.Bouquets.ToList();
        }

        public Bouquet GetOneBouquetById(int Id)
        {
            return _context.Bouquets.Find(Id);
        }

        public void Delete(int Id)
        {
            var bouquet = _context.Bouquets.Find(Id);
            if (bouquet == null)
            {
               //throw new NotFoundException();
               
            }

            _context.Bouquets.Remove(bouquet);
            _context.SaveChanges();
        }

        public Bouquet Insert(int Id, string Name, int Price, string Description)
        {
            //CheckBouquetExists(Id);
            var bouquet = new Bouquet
            {
               Id = Id,
               Name = Name,
               Price = Price,
               Description = Description
            };
            _context.Bouquets.Add(bouquet);
            _context.SaveChanges();
            return bouquet;
        }

        public Bouquet Update(int Id, string Name, int Price, string Description)
        {
            var bouquet = _context.Bouquets.Find(Id);
            if (bouquet == null)
            {
               //throw  new NotFoundException();
               
            }

            bouquet.Name = Name;
            bouquet.Price = Price;
            bouquet.Description = Description;
            _context.SaveChanges();
            return bouquet;
        }
    }
}