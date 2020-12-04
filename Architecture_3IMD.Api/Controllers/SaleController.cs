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
        private readonly ISalesRepository _salesRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<SaleController> _logger;

        public SaleController(ILogger<SaleController> logger, ISalesRepository salesRepository, IMemoryCache memoryCache)
        {
            _salesRepository = salesRepository;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        ///<summary>
        /// Gets a list of all the sales.
        ///</summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET Flowershop/Sale
        ///     {   
        ///       "Id": "5fc8d4bd026533d540f873aa",    
        ///       "Store_id": 1,
        ///       "Bouquet_id": 1,
        ///       "Amount": 10,
        ///       "FirstName": "John",
        ///       "LastName": "Doe"       
        ///     }
        /// </remarks>   
        /// <response code="200">If GET request was successfully executed.</response>
        [HttpGet]
        public async Task<IActionResult> GetSales() => Ok(await GetAllSalesFromCacheAsync());

        ///<summary>
        /// Gets a single sale.
        ///</summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET Flowershop/Sale
        ///     {   
        ///       "Id": "5fc8d4bd026533d540f873aa",    
        ///       "Store_id": 1,
        ///       "Bouquet_id": 1,
        ///       "Amount": 10,
        ///       "FirstName": "John",
        ///       "LastName": "Doe" 
        ///     }
        /// </remarks>  
        /// <param name="Id">The unique identifier of the sale</param> 
        /// <response code="200">If GET request was successfully executed.</response>
        [HttpGet("{Id}", Name = nameof(GetSale))]
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
        /// Creates a new sale.
        ///</summary>
        ///<remarks>
        /// Sample request:
        ///
        ///     POST Flowershop/Sale
        ///     {        
        ///       "Store_id": 1,
        ///       "Bouquet_id": 1,
        ///       "Amount": 10,
        ///       "FirstName": "John",
        ///       "LastName": "Doe"         
        ///     }
        /// </remarks>
        /// <response code="201">If new sale was successfully created.</response>
        /// <response code="400">If one or more required fields are null.</response>   
        [HttpPost(Name = nameof(CreateSale))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSale([FromBody] CreateSaleDto saleDto)
        {
            var sale = new Sale
            {
               Store_id = saleDto.Store_id,
               Bouquet_id = saleDto.Bouquet_id,
               Amount = saleDto.Amount,
               FirstName = saleDto.FirstName,
               LastName = saleDto.LastName
            };
            await _salesRepository.InsertSaleAsync(sale);
            _memoryCache.Remove(CacheKeys.AllSales);
            // Debatable if you want to do a fetch from the cache or a fetch directly from the db. This all depends on your configuration
            var returningSale = (await GetAllSalesFromCacheAsync()).FirstOrDefault(x => x.Id == sale.Id);
            return base.CreatedAtRoute(nameof(GetSale), new { Id = sale.Id }, returningSale);
        }

        private Task<List<ViewSaleDto>> GetAllSalesFromCacheAsync() =>
            _memoryCache.GetOrCreateAsync(CacheKeys.AllSales, entry => GetAllSalesAsViewDto());

        private async Task<List<ViewSaleDto>> GetAllSalesAsViewDto()
        {
            var sales = await _salesRepository.GetAllSalesAsync();
            return sales.Select(x => new ViewSaleDto(x, new SaleLinks(Url.Link(nameof(GetSale), new { Id = x.Id })))).ToList();
        }

    }
}