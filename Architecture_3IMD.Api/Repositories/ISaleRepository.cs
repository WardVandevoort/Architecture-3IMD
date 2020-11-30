using System.Collections.Generic;
using System.Threading.Tasks;
using Architecture_3IMD.Models;
using Architecture_3IMD.Models.Domain;

namespace Architecture_3IMD.Repositories
{
    public interface ISaleRepository
    {
        Task<List<Sale>> GetAllSalesAsync();
        Task InsertSaleAsync(Sale sale);
    }
}