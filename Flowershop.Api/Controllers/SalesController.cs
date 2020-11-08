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
     [Route("Flowershop/[controller]")]
     [ApiController]
     public class SaleController : Controller
     {          
          private readonly ISalesRepository _salesRepository;
          private readonly ILogger<SaleController> _logger;

          public SaleController(ISalesRepository salesRepository, ILogger<SaleController> logger)
          {
               _salesRepository = salesRepository;
               _logger = logger;
          }

          /// <summary>
          /// Gets a list of all the stores and the amount of sales per bouquet.
          /// </summary>
          /// <remarks>
          /// Sample request:
          /// 
          ///     GET Flowershop/Sale
          ///     {   
          ///       "Id": 1,    
          ///       "Store_id": 1,
          ///       "Bouquet_id": 1,
          ///       "Amount": 10        
          ///     }
          /// </remarks>   
          /// <response code="200">If GET request was successfully executed.</response>
          [HttpGet]
          [ProducesResponseType(200)]
          public async Task<IActionResult> getAllSales()
          {
               _logger.LogInformation("Getting all sales");
               
               //  Code that gets all the stores.
               var sales = await _salesRepository.GetAllSales();
               return Ok(sales);
          }

          /// <summary>
          /// Creates a new store/bouquet sale combination.
          /// </summary>
          /// <remarks>
          /// Sample request:
          /// 
          ///     POST Flowershop/Sale
          ///     {        
          ///       "Store_id": 1,
          ///       "Bouquet_id": 1      
          ///     }
          /// </remarks>
          /// <response code="200">If new combination was successfully added.</response>
          /// <response code="400">If one or more required fields are null.</response>   
          [HttpPost]
          [ProducesResponseType(200)]
          [ProducesResponseType(400)]
          public async Task<IActionResult> createSaleCombination([FromBody]Sale sale)
          {
               _logger.LogInformation("Adding a sale combination", sale);

               //   Code that creates a new sale combination.
               await _salesRepository.Insert(sale.Id, sale.Store_id, sale.Bouquet_id, sale.Amount);
               return Content("Sale combination added");
          }

          /// <summary>
          /// Adds a new sale.
          /// </summary>
          /// <remarks>
          /// Sample request:
          /// 
          ///     PATCH Flowershop/Sale
          ///     {        
          ///       "Store_id": 1,
          ///       "Bouquet_id": 1,
          ///       "Amount": (default is 1 if no amount is specified)     
          ///     }
          /// </remarks>
          /// <response code="200">If new sale was successfully added.</response>
          /// <response code="400">If one or more required fields are null.</response>   
          [HttpPatch]
          [ProducesResponseType(200)]
          [ProducesResponseType(400)]
          public async Task<IActionResult> addSale([FromBody]Sale sale)
          {
               _logger.LogInformation("Adding a new sale", sale);

               //   Code that adds a new sale.
              
               await _salesRepository.Update(sale.Id, sale.Store_id, sale.Bouquet_id, sale.Amount);
               
               return Content("New sale added");
          }

    }
}

