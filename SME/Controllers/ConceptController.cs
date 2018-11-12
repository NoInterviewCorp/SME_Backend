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
        /// Retrieves All concepts from the Database
        /// </summary>
        /// <response code="200">Returns all concepts </response>
        // GET Resource/
        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult GetConcepts()
        {
            var concepts = repository.GetConcepts();
            if (concepts == null)
            {
                concepts = new List<Concept>();
            }
            return Ok(concepts);
        }

        /// <summary>
        /// Retrieves All concepts from the Database
        /// </summary>
        /// <response code="200">Returns all concepts </response>
        /// <response code="404">Concept not found </response>
        // GET Resource/
        [HttpGet("{conceptName}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult GetConcepts(string conceptName)
        {
            var concepts = repository.GetConceptByName(conceptName);
            if (concepts == null)
            {
                return NotFound();
            }
            return Ok(concepts);
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
                    return Created("/concept", conceptObj);
                }
            }
            return BadRequest();
        }
        /// <summary>
        /// Updates a <paramref name="concept"/> into the database if it exists
        /// </summary>
        /// <param name="concept">Concept to be added. </param>
        /// <param name="ConceptId">Conceptid of object which needs to be updated. </param>
        /// <response code="201">Returns the newly updated Concept</response>
        /// <response code="400">If the Concept does not exists or modelstate is invalid </response> 
        // PUT SME/
        [HttpPut("{ConceptId}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult Put(string ConceptId, [FromBody] Concept concept)
        {
            if (ModelState.IsValid)
            {
                var ConceptObj = repository.UpdateConcept(concept);
                if (ConceptObj == null)
                {
                    return NotFound(concept.Name + " was not found or You didn't include it's ID");
                }
                else
                {
                    return Created("/concept", ConceptObj);
                }
            }
            return BadRequest();
        }
    }
}