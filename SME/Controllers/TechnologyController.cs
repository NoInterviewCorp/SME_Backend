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

        /// <summary>
        /// Retrieves a technology from the Database by its name
        /// </summary>
        /// <response code="200">Returns a technology </response>
        /// <response code="404">technology not found </response>
        // GET Resource/
        [HttpGet("{technologyName}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult GetTechnology(string technologyName)
        {
            var technology = repository.GetTechnologyByName(technologyName);
            if (technology == null)
            {
                return NotFound();
            }
            return Ok(technology);
        }

        /// <summary>
        /// Posts a <paramref name="technology"/> into the database
        /// </summary>
        /// <param name="technology"> Object of type technology which needs to be posted
        /// to the database </param>
        /// <response code="201">Returns the newly created technology</response>
        /// <response code="400">If the technology already exists or modelstate is invalid </response> 
        // POST technology/
        [HttpPost()]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult Post([FromBody] Technology technology)
        {
            if (ModelState.IsValid)
            {
                var technologyObj = repository.AddTechnology(technology);
                if (technologyObj == null)
                {
                    return BadRequest("technology submitted is invalid");
                }
                else
                {
                    return Created("/technology", technologyObj);
                }
            }
            return BadRequest();
        }

        /// <summary>
        /// Updates a <paramref name="technology"/> into the database if it exists
        /// </summary>
        /// <param name="technology">Technology to be added. </param>
        /// <param name="technologyId">technologyId of object which needs to be updated. </param>
        /// <response code="201">Returns the newly updated Technology</response>
        /// <response code="400">If the Technology does not exists or modelstate is invalid </response> 
        // PUT SME/
        [HttpPut("{technologyId}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult Put(string technologyId, [FromBody] Technology technology)
        {
            if (ModelState.IsValid)
            {
                var technologyObj = repository.UpdateTechnology(technology);
                if (technologyObj == null)
                {
                    return NotFound(technology.Name + " was not found or You didn't include it's ID");
                }
                else
                {
                    return Created("/technology", technologyObj);
                }
            }
            return BadRequest();
        }
    }
}