using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Architecture_3IMD.Models.Domain;
using Architecture_3IMD.Models.Web;
using Architecture_3IMD.Data;
using Architecture_3IMD.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Architecture_3IMD.Controllers
{
    [ApiController]
    [Route("Flowershop/[controller]")]
    [Produces("application/json")]
    public class SaleController : Controller
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<SaleController> _logger;

        public SaleController(ILogger<SaleController> logger, ISaleRepository saleRepository, IMemoryCache memoryCache)
        {
            _saleRepository = saleRepository;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        ///<summary>
        /// Get all sales
        ///</summary>
        //[HttpGet("/Sales", Name = nameof(GetCats))]
        [HttpGet]
        public async Task<IActionResult> GetSales() => Ok(await GetAllSalesFromCacheAsync());

        ///<summary>
        /// Get a single sale by id.
        ///</summary>
        ///<param name="Id">The primary key of the sale object.</param>
        [HttpGet("/sales/{Id}", Name = nameof(GetSale))]
        public async Task<IActionResult> GetSale(string Id)
        {
            var sales = await GetAllSalesFromCacheAsync();
            var sale = sales.FirstOrDefault(x => x.Id == Id);
            if (sale == null)
            {
                return NotFound();
            }
            return Ok(sale);
        }

        ///<summary>
        /// Creates a new sale
        ///</summary>
        ///<remarks>
        /// Sample request:
        ///
        ///     POST /dogs
        ///     {
        ///        "Name": "Jeff",
        ///        "PottyTrained": true,
        ///        "Barks": true
        ///     }
        /// </remarks>
        [HttpPost("/sales", Name = nameof(CreateSale))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSale([FromBody] CreateSaleDto saleDto)
        {
            var sale = new Sale
            {
               Id = saleDto.Id,
               Store_id = saleDto.Store_id,
               Bouquet_id = saleDto.Bouquet_id,
               Amount = saleDto.Amount,
               FirstName = saleDto.FirstName,
               LastName = saleDto.LastName
            };
            await _saleRepository.InsertSaleAsync(sale);
            _memoryCache.Remove(CacheKeys.AllSales);
            // Debatable if you want to do a fetch from the cache or a fetch directly from the db. This all depends on your configuration
            var returningSale = (await GetAllSalesFromCacheAsync()).FirstOrDefault(x => x.Id == sale.Id);
            return base.CreatedAtRoute(nameof(GetSale), new { Id = sale.Id }, returningSale);
        }

        private Task<List<ViewSaleDto>> GetAllSalesFromCacheAsync() =>
            _memoryCache.GetOrCreateAsync(CacheKeys.AllSales, entry => GetAllSalesAsViewDto());

        private async Task<List<ViewSaleDto>> GetAllSalesAsViewDto()
        {
            var sales = await _saleRepository.GetAllSalesAsync();
            return sales.Select(x => new ViewSaleDto(x, new SaleLinks(Url.Link(nameof(GetSale), new { Id = x.Id })))).ToList();
        }

    }
}