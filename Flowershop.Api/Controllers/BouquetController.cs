﻿using System;
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
    public class BouquetController : Controller
    {

        private readonly IBouquetsRepository _bouquetsRepository;

        private readonly ILogger<BouquetController> _logger;

        public BouquetController(IBouquetsRepository bouquetsRepository, ILogger<BouquetController> logger)
        {
            _bouquetsRepository = bouquetsRepository;
            _logger = logger;
        }

        /// <summary>
        /// Gets a list of all the bouquets.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET Flowershop/Bouquet
        ///     {   
        ///       "Id": 1,    
        ///       "Name": "Roses",
        ///       "Price": 20,
        ///       "Description": "A bouquet of red roses"        
        ///     }
        /// </remarks>   
        /// <response code="200">If GET request was successfully executed.</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> getAllBouquets()
        {
            _logger.LogInformation("Getting all bouquets");
            
            //  Code that gets all the bouquets.
            var bouquets = await _bouquetsRepository.GetAllBouquets();
            return Ok(bouquets);
        }

        /// <summary>
        /// Creates a new bouquet.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST Flowershop/Bouquet
        ///     {        
        ///       "Name": "Roses",
        ///       "Price": 20,
        ///       "Description": "A bouquet of red roses"        
        ///     }
        /// </remarks>
        /// <response code="200">If new bouquet was successfully created.</response>
        /// <response code="400">If one or more required fields are null.</response>   
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> createBouquet([FromBody]Bouquet bouquet)
        {
            _logger.LogInformation("Creating a bouquet", bouquet);

            //  Code that creates a new bouquet.
            await _bouquetsRepository.Insert(bouquet.Id, bouquet.Name, bouquet.Price, bouquet.Description);
            return Content("Bouquet created");
        }

    }
}
