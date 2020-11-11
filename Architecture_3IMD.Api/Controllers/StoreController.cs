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
          [ProducesResponseType(typeof(IEnumerable<StoreWebOutput>), StatusCodes.Status200OK)]
          public async Task<IActionResult> getAllStores()
          {
               _logger.LogInformation("Getting all stores");
               
               //  Code that gets all the stores.
               var stores = await _storesRepository.GetAllStores();
               return Ok(stores);
          }

          /// <summary>
          /// Gets a single store.
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
          /// <param name="Id">The unique identifier of the store</param>
          /// <response code="200">If GET request was successfully executed.</response>
          [HttpGet("{Id}")]
          [ProducesResponseType(typeof(StoreWebOutput), StatusCodes.Status200OK)]
          public async Task<IActionResult> getOneStore(int Id)
          {
               _logger.LogInformation("Getting store by id", Id);
               var store = await _storesRepository.GetOneStoreById(Id);
               return store == null ? (IActionResult) NotFound() : Ok(store);
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
          /// <response code="201">If new store was successfully created.</response>
          /// <response code="400">If one or more required fields are null.</response>   
          [HttpPost]
          [ProducesResponseType(typeof(StoreWebOutput),StatusCodes.Status201Created)]
          [ProducesDefaultResponseType]
          public async Task<IActionResult> createStore(StoreUpsertInput store)
          {
               _logger.LogInformation("Creating a store", store);

               //   Code that creates a new store.
               var persistedStore = await _storesRepository.Insert(store.Id, store.Name, store.Address, store.Region);
               return Created($"/stores/{persistedStore.Id}", persistedStore);
          }

    }
}

