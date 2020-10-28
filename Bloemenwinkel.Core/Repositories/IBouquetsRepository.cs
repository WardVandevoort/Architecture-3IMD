using System.Collections.Generic;
using Architecture_3IMD.Models.Domain;

namespace Architecture_3IMD.Repositories
{
    public interface IBouquetsRepository
    {
        IEnumerable<Bouquet> GetAllBouquets();
        Bouquet GetOneBouquetById(int Id);
        void Delete(int Id);
        Bouquet Insert(int Id, string Name, int Price, string Description);
        Bouquet Update(int Id, string Name, int Price, string Description);
    }
}