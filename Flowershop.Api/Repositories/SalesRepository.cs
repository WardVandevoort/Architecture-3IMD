using System.Collections.Generic;
using System.Linq;
using Architecture_3IMD.Data;
using Architecture_3IMD.Models;
using Architecture_3IMD.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Architecture_3IMD.Repositories
{
    public class SalesRepository : ISalesRepository
    {
        private readonly ApplicationDbContext _context;

        public SalesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Sale>> GetAllSales()
        {
            return await _context.Sales.ToListAsync();
        }

        public async Task<IEnumerable<Sale>> GetBestSellingBouquets()
        {
            var array = await _context.Sales.ToArrayAsync();
            return array;
        }

        public async Task<Sale>  GetOneSaleById(int Store_id, int Bouquet_id)
        {
             var sale = await _context.Sales.FirstOrDefaultAsync(x => x.Store_id == Store_id && x.Bouquet_id == Bouquet_id);
             return sale;
        }

        public async Task Delete(int Id)
        {
            var sale = await _context.Sales.FindAsync(Id);
            if (sale == null)
            {
               //throw new NotFoundException();
               
            }

            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync();
        }

        public async Task<Sale> Insert(int Id, int Store_id, int Bouquet_id, int Amount)
        {
            var sale = new Sale
            {
               Id = Id,
               Store_id = Store_id,
               Bouquet_id = Bouquet_id,
               Amount = Amount
            };
            await _context.Sales.AddAsync(sale);
            await _context.SaveChangesAsync();
            return sale;
        }

        public async Task<Sale> Update(int Id, int Store_id, int Bouquet_id, int Amount)
        {
            var sale = await GetOneSaleById(Store_id, Bouquet_id);

            if( Amount == 0 ){
               sale.Amount++;
            }
            else{
               sale.Amount = sale.Amount + Amount;
            }

            await _context.SaveChangesAsync();
            return sale;
        }
    }
}