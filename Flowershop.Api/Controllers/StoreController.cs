using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Architecture_3IMD.Models.Domain;
using Architecture_3IMD.Models.Web;
using Architecture_3IMD.Data;
using Architecture_3IMD.Repositories;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Http;

namespace Architecture_3IMD.Controllers
{
     [Route("Flowershop/[controller]")]
     [ApiController]
     public class StoreController : Controller
     {          
          private readonly IStoresRepository _storesRepository;
          private readonly ILogger<StoreController> _logger;

          public StoreController(IStoresRepository storesRepository, ILogger<StoreController> logger)
          {
               _storesRepository = storesRepository;
               _logger = logger;
          }

          /// <summary>
          /// Gets a list of all the stores.
          /// </summary>
          /// <remarks>
          /// Sample request:
          /// 
          ///     GET Flowershop/Store
          ///     {    
          ///       "Id": 1,    
          ///       "Name": "Fleurtop",
          ///       "Address": "Steestraat 15",
          ///       "Region": "Tremelo"        
          ///     }
          /// </remarks>
          /// <response code="200">If GET request was successfully executed.</response>
          [HttpGet]
          [ProducesResponseType(200)]
          public async Task<IActionResult> getAllStores()
          {
               _logger.LogInformation("Getting all stores");
               
               //  Code that gets all the stores.
               var stores = await _storesRepository.GetAllStores();
               return Ok(stores);
          }

          /// <summary>
          /// Creates a new store.
          /// </summary>
          /// <remarks>
          /// Sample request:
          /// 
          ///     POST Flowershop/Store
          ///     {        
          ///       "Name": "Fleurtop",
          ///       "Address": "Steestraat 15",
          ///       "Region": "Tremelo"        
          ///     }
          /// </remarks>
          /// <response code="200">If new store was successfully created.</response>
          /// <response code="400">If one or more required fields are null.</response>   
          [HttpPost]
          [ProducesResponseType(200)]
          [ProducesResponseType(400)]
          [ProducesResponseType(typeof(StoreWebOutput),StatusCodes.Status201Created)]
          public async Task<IActionResult> createStore(StoreUpsertInput store)
          {
               _logger.LogInformation("Creating a store", store);

               //   Code that creates a new store.
               var persistedStore = await _storesRepository.Insert(store.Id, store.Name, store.Address, store.Region);
               return Created($"/stores/{persistedStore.Id}", persistedStore);
          }

    }
}

