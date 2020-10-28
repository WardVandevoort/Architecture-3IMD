using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Architecture_3IMD.Models.Domain;
using Architecture_3IMD.Data;
using Architecture_3IMD.Repositories;

using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Http;




namespace Architecture_3IMD.Controllers
{
    //[Route("/api/[controller]")]
    public class HomeController : Controller
    {

        //private ApplicationDbContext db = new ApplicationDbContext();
        private readonly IBouquetsRepository _bouquetsRepository;
        private readonly IStoresRepository _storesRepository;


        private readonly ILogger<HomeController> _logger;

        public HomeController(IStoresRepository storesRepository, IBouquetsRepository bouquetsRepository, ILogger<HomeController> logger)
        {
            _bouquetsRepository = bouquetsRepository;
            _storesRepository = storesRepository;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return Content("Home");
        }

        [HttpGet]
        public IActionResult getAllStores()
        {
            _logger.LogInformation("Getting all stores");
            // This is a linq extension method: https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.select?view=netcore-3.1
            var stores = _storesRepository.GetAllStores();
            return Ok(stores);
        }

        [HttpGet("{id}")]
        public IActionResult getStore([FromBody] int id)
        {
            return Content("This is store" + id);
        }

        [HttpPost]
        public async Task<IActionResult> postTest([FromBody] Stores model)
        {
            if(model == null){
                return StatusCode(400);
            }
            
            return StatusCode(200);
            
            
        }

        [HttpGet]
        public IActionResult getAllBouquets()
        {
            _logger.LogInformation("Getting all bouquets");
            // This is a linq extension method: https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.select?view=netcore-3.1
            var bouquets = _bouquetsRepository.GetAllBouquets();
            return Ok(bouquets);

        }
        
        public IActionResult Privacy()
        {
            return View();
        }

        /*[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }*/
    }
}

