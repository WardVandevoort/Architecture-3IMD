using System.Collections.Generic;
using Architecture_3IMD.Models.Domain;

namespace Architecture_3IMD.Repositories
{
    public interface IStoresRepository
    {
        IEnumerable<Stores> GetAllStores();
        Stores GetOneStoreById(int Id);
        void Delete(int Id);
        Stores Insert(int Id, string Name, string Address, string Region);
        Stores Update(int Id, string Name, string Address, string Region);
    }
}