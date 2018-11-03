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
    public class ConceptController : ControllerBase
    {
        // dependency injection for the repository interface
        // which is responsible for business logic for interacting
        // with the database
        private IDatabaseRepository repository;

        public ConceptController(IDatabaseRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Retrieves All Resources from the Database
        /// </summary>
        /// <response code="200">Returns all Resources </response>
        // GET Resource/
        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult GetConcepts()
        {
            var resources = repository.GetConcepts();
            if (resources == null)
            {
                resources = new List<Concept>();
            }
            return Ok(resources);
        }

        /// <summary>
        /// Posts a <paramref name="concept"/> into the database
        /// </summary>
        /// <param name="concept"> Object of type concept which needs to be posted
        /// to the database </param>
        /// <response code="201">Returns the newly created concept</response>
        /// <response code="400">If the concept already exists or modelstate is invalid </response> 
        // POST concept/
        [HttpPost()]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult Post([FromBody] Concept concept)
        {
            if (ModelState.IsValid)
            {
                var conceptObj = repository.AddConcept(concept);
                if (conceptObj == null)
                {
                    return BadRequest("Concept submitted is invalid");
                }
                else
                {
                    return Created("concept", conceptObj);
                }
            }
            return BadRequest();
        }
    }
}