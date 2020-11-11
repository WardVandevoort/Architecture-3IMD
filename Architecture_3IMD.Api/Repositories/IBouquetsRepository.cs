using System.Collections.Generic;
using Architecture_3IMD.Models.Domain;
using System.Threading.Tasks;

namespace Architecture_3IMD.Repositories
{
    public interface IBouquetsRepository
    {
        Task<IEnumerable<Bouquet>> GetAllBouquets();
        Task<Bouquet> GetOneBouquetById(int Id);
        Task Delete(int Id);
        Task<Bouquet> Insert(int Id, string Name, int Price, string Description);
        Task<Bouquet> Update(int Id, string Name, int Price, string Description);
    }
}