using System.Collections.Generic;
using Architecture_3IMD.Models.Domain;
using System.Threading.Tasks;

namespace Architecture_3IMD.Repositories
{
    public interface IStoresRepository
    {
        Task<IEnumerable<Store>> GetAllStores();
        Task<Store> GetOneStoreById(int Id);
        Task Delete(int Id);
        Task<Store> Insert(int Id, string Name, string Address, string StreetNumber, string Region);
        Task<Store> Update(int Id, string Name, string Address, string StreetNumber, string Region);
    }
}