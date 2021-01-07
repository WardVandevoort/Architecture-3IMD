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
using System.Collections;

namespace Architecture_3IMD.Controllers
{
    [ApiController]
    [Route("Flowershop/[controller]")]
    [Produces("application/json")]
    public class SaleController : Controller
    {
        private readonly ISalesRepository _salesRepository;
        private readonly IBouquetsRepository _bouquetsRepository;
        private readonly IStoresRepository _storesRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<SaleController> _logger;

        public SaleController(ILogger<SaleController> logger, ISalesRepository salesRepository, IMemoryCache memoryCache, IBouquetsRepository bouquetsRepository, IStoresRepository storesRepository)
        {
            _salesRepository = salesRepository;
            _bouquetsRepository = bouquetsRepository;
            _storesRepository = storesRepository;
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
        /// Gets an overview of the best selling bouquets.
        ///</summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET Flowershop/Sale/Overview
        ///     {   
        ///       "Bouquet_id": 1,
        ///       "TotalAmountSold": 100    
        ///     }
        /// </remarks>   
        /// <response code="200">If GET request was successfully executed.</response>
        [HttpGet("Overview")]
        public async Task<IActionResult> GetSalesOverview()
        {
        var sales = await GetAllSalesFromCacheAsync();
        var overview = from sale in sales
                       group sale by sale.Bouquet_id into bouquetOverview
                       select new
                       {
                            Bouquet_id = bouquetOverview.Key,
                            TotalAmountSold = bouquetOverview.Sum(x => x.Amount),
                       };
                       
        overview = overview.OrderByDescending(bouquetOverview => bouquetOverview.TotalAmountSold);
        return Ok(overview);
        }

        ///<summary>
        /// Gets an overview of the best selling bouquets for a specific store.
        ///</summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET Flowershop/Sale/OverviewBouquetByStore
        ///     {   
        ///       "Bouquet_id": 1,
        ///       "TotalAmountSold": 10      
        ///     }
        /// </remarks>   
        /// <param name="store_id">The unique identifier of the store</param> 
        /// <response code="200">If GET request was successfully executed.</response>
        [HttpGet("OverviewBouquetByStore/{store_id}")]
        public async Task<IActionResult> GetBouquetByStoreOverview(int store_id)
        {
        var sales = await GetAllSalesFromCacheAsync();
        var lists = from sale in sales
                    where sale.Store_id == store_id
                    select sale;
        var overview = from list in lists
                       group list by list.Bouquet_id into StoreOverview
                       select new
                       {
                            Bouquet_id = StoreOverview.Key,
                            TotalAmountSold = StoreOverview.Sum(x => x.Amount),
                       };
                       
        overview = overview.OrderByDescending(StoreOverview => StoreOverview.TotalAmountSold);
        return Ok(overview);
        }

        ///<summary>
        /// Gets an overview of the revenue of all the stores.
        ///</summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET Flowershop/Sale/OverviewRevenue
        ///     {   
        ///       "Store_id": 1,
        ///       "TotalRevenue": 100    
        ///     }
        /// </remarks>   
        /// <response code="200">If GET request was successfully executed.</response>
        [HttpGet("OverviewRevenue")]
        public async Task<IActionResult> GetRevenueOverview()
        {
        var sales = await GetAllSalesFromCacheAsync();
        var bouquets = await _bouquetsRepository.GetAllBouquets();
        
        var saleList = from sale in sales
                    group sale by new { sale.Store_id, sale.Bouquet_id }
                    into grp
                    select new
                    {
                        
                        grp.Key.Store_id,
                        grp.Key.Bouquet_id,
                        AmountSold = grp.Sum(x => x.Amount),
                    };

        var revenueLists = new List<Revenue>();

        foreach (var item in saleList)
        {
            var price = from bouquet in bouquets
                        where bouquet.Id == item.Bouquet_id
                        select bouquet.Price;
            int bouquetPrice = price.FirstOrDefault();
            
            int revenue = item.AmountSold * bouquetPrice;
            revenueLists.Add(new Revenue() {Store_id = item.Store_id, Bouquet_id = item.Bouquet_id, BouquetRevenue = revenue});
            
        } 

        var list = from revenueList in revenueLists
                    group revenueList by revenueList.Store_id into RevenueOverview
                       select new
                       {
                            Store_id = RevenueOverview.Key,
                            TotalRevenue = RevenueOverview.Sum(x => x.BouquetRevenue),
                       };

        list = list.OrderByDescending(RevenueOverview => RevenueOverview.TotalRevenue);

        return Ok(list);
        }

        ///<summary>
        /// Gets a sales overview of stores in the same region.
        ///</summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET Flowershop/Sale/RegionOverview
        ///     {   
        ///       "Store_id": 1,
        ///       "TotalAmountSold": 100      
        ///     }
        /// </remarks>   
        /// <param name="region">The region in which store(s) are located</param> 
        /// <response code="200">If GET request was successfully executed.</response>
        [HttpGet("RegionOverview/{region}")]
        public async Task<IActionResult> GetRegionOverview(string region)
        {
        var sales = await GetAllSalesFromCacheAsync();
        var stores = await _storesRepository.GetAllStores();
        
        var storesOverview = from store in stores
                    where store.Region == region
                    select store;

        var saleLists = new List<SaleByRegion>();

        foreach (var sale in sales)
        {
            foreach (var item in storesOverview)
            {
                if(sale.Store_id == item.Id){
                    saleLists.Add(new SaleByRegion() {Store_id = sale.Store_id, Bouquet_id = sale.Bouquet_id, Amount = sale.Amount});
            
                }

            }  

        }
        
        var overview = from saleList in saleLists
                       group saleList by saleList.Store_id into SaleOverview
                       select new
                       {
                            Store_id = SaleOverview.Key,
                            TotalAmountSold = SaleOverview.Sum(x => x.Amount),
                       };

        overview = overview.OrderByDescending(overview => overview.TotalAmountSold);
        return Ok(overview); 
                       
        }

        //bouquet by store overview
        //store overview
        //region overview

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