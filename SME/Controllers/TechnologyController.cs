using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SME.Models;
using SME.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace SME.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TechnologyController : ControllerBase
    {
        // dependency injection for the repository interface
        // which is responsible for business logic for interacting
        // with the database
        private IDatabaseRepository repository;

        public TechnologyController(IDatabaseRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Retrieves All Technologies from the Database
        /// </summary>
        /// <response code="200">Returns all Technologies </response>
        // GET Resource/
        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult GetTechnologies()
        {
            var resources = repository.GetAllTechnologies();
            if (resources == null)
            {
                resources = new List<Technology>();
            }
            return Ok(resources);
        }
    }
}