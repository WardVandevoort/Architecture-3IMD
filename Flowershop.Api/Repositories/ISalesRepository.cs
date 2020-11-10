using System.Collections.Generic;
using Architecture_3IMD.Models.Domain;
using System.Threading.Tasks;

namespace Architecture_3IMD.Repositories
{
    public interface ISalesRepository
    {
        Task<IEnumerable<Sale>> GetAllSales();
        Task<IEnumerable<Sale>> GetBestSellingBouquets();
        Task<Sale> GetOneSaleById(int Store_id, int Bouquet_id);
        Task Delete(int Id);
        Task<Sale> Insert(int Id, int Store_id, int Bouquet_id, int Amount);
        Task<Sale> Update(int Id, int Store_id, int Bouquet_id, int Amount);
    }
}