using System.Collections.Generic;
using Architecture_3IMD.Models.Domain;
using System.Threading.Tasks;

namespace Architecture_3IMD.Repositories
{
    public interface IStoresRepository
    {
        Task<IEnumerable<Stores>> GetAllStores();
        Task<Stores> GetOneStoreById(int Id);
        Task Delete(int Id);
        Task<Stores> Insert(int Id, string Name, string Address, string Region);
        Task<Stores> Update(int Id, string Name, string Address, string Region);
    }
}