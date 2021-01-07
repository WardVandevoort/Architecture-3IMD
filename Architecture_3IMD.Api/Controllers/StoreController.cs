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
using BasisRegisters.Vlaanderen;

namespace Architecture_3IMD.Controllers
{
     [Route("Flowershop/[controller]")]
     [ApiController]
     public class StoreController : Controller
     {          
          private readonly IStoresRepository _storesRepository;
          private readonly ILogger<StoreController> _logger;
          private readonly IBasisRegisterService _basisRegisterService;

          public StoreController(IStoresRepository storesRepository, ILogger<StoreController> logger, IBasisRegisterService basisRegisterService)
          {
               _storesRepository = storesRepository;
               _logger = logger;
               _basisRegisterService = basisRegisterService;
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
          ///       "Address": "Steestraat",
          ///       "StreetNumber": "100",
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
          ///       "Address": "Steestraat",
          ///       "StreetNumber": "100",
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
          ///       "Address": "Steestraat",
          ///       "StreetNumber": "100",
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

                    // Code that checks if the given address exists
                    var addresses = await _basisRegisterService
                    .AddressMatchAsync(store.Region, null, null, null, null, store.Address, store.StreetNumber, null, null);
                    addresses.Warnings.ToList().ForEach(x => _logger.LogWarning($"{x.Code} {x.Message}"));
                    if(!addresses.Warnings.Any()){
                         // Code that creates a new store.
                         _logger.LogInformation("Creating a store", store);
                         var persistedStore = await _storesRepository.Insert(store.Id, store.Name, store.Address, store.StreetNumber, store.Region);
                         return Created($"/stores/{persistedStore.Id}", persistedStore);
                    }
                    else{
                         return Ok("The given address does not exist!");
                    }

          }

          /// <summary>
          /// Updates a store.
          /// </summary>
          /// <remarks>
          /// Sample request:
          /// 
          ///     PATCH Flowershop/Store
          ///     {    
          ///       "Id": 1,    
          ///       "Name": "Fleurtop",
          ///       "Address": "Steestraat",
          ///       "StreetNumber": "100",
          ///       "Region": "Tremelo"        
          ///     }
          /// </remarks>
          /// <param name="Id">The unique identifier of the store</param>
          /// <response code="202">If store was successfully updated.</response>
          /// <response code="400">If one or more required fields are null.</response>   
          [HttpPatch("{Id}")]
          [ProducesResponseType(typeof(StoreWebOutput),StatusCodes.Status202Accepted)]
          [ProducesDefaultResponseType]
          public async Task<IActionResult> updateStore(int Id, StoreUpsertInput store)
          {
          
                    // Code that checks if the given address exists
                    var addresses = await _basisRegisterService
                    .AddressMatchAsync(store.Region, null, null, null, null, store.Address, store.StreetNumber, null, null);
                    addresses.Warnings.ToList().ForEach(x => _logger.LogWarning($"{x.Code} {x.Message}"));
                    if(!addresses.Warnings.Any()){
                         // Code that updates a store.
                         _logger.LogInformation("Updating a store", store);
                         var persistedStore = await _storesRepository.Update(store.Id, store.Name, store.Address, store.StreetNumber, store.Region);
                         return Accepted();
                    }
                    else{
                         return Ok("The given address does not exist!");
                    }
              
          }

          /// <summary>
          /// Deletes a store.
          /// </summary>
          /// <remarks>
          /// Sample request:
          /// 
          ///     DELETE Flowershop/Store
          ///     {        
          ///       "Id": 1,      
          ///     }
          /// </remarks>
          /// <param name="Id">The unique identifier of the store</param>
          /// <response code="204">If store was successfully deleted.</response>
          /// <response code="400">If one or more required fields are null.</response>   
          [HttpDelete("{Id}")]
          [ProducesResponseType(typeof(StoreWebOutput),StatusCodes.Status204NoContent)]
          [ProducesDefaultResponseType]
          public async Task<IActionResult> deleteStore(int Id)
          {
               try
               {
                    // Code that deletes a store.
                    _logger.LogInformation("Deleting a store");
                    await _storesRepository.Delete(Id);
                    return NoContent();
               }
               catch (Exception ex)
               {
                    _logger.LogCritical(ex, "Error");
                    return BadRequest();
               }

          }

    }
}

