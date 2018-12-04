using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SME.Models;
using SME.Persistence;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace SME.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ResourceController : ControllerBase
    {
        // dependency injection for the repository interface
        // which is responsible for business logic for interacting
        // with the database
        private IResourceRepository repository;

        public ResourceController(IResourceRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Retrieves All Resources from the Database
        /// </summary>
        /// <response code="200">Returns all Resources </response>
        /// <response code="404">Didn't find any resource</response>
        // GET Resource/
        [HttpGet("{authorId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetResourcesAsync(string authorId)
        {
            var resources = await repository.GetResourceByAuthorIdAsync(authorId);
            if (resources == null)
            {
                NotFound();
            }
            return Ok(resources);
        }

        /// <summary>
        /// Retrieves a resource from the Database which belongs to technology
        /// or by a resource's link 
        /// </summary>
        /// <param name="text">could be technology/resourcelink for which we need all resources</param>
        /// <response code="200">Returns all resource </response>
        /// <response code="404">Resource not found </response>
        // GET Resource/
        [HttpGet("{text}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetResource(string text)
        {
            var resources = await repository.GetResourceByStringAsync(text);
            if (resources == null)
            {
                return NotFound();
            }
            return Ok(resources);
        }

        /// <summary>
        /// Posts a <paramref name="resource"/> into the database
        /// </summary>
        /// <param name="resource"> Object of type resource which needs to be posted
        /// to the database </param>
        /// <response code="201">Returns the newly created resource</response>
        /// <response code="400">If the resource already exists or modelstate is invalid </response> 
        // POST SME/
        [HttpPost()]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PostAsync([FromBody] Resource resource)
        {
            if (ModelState.IsValid)
            {
                var resourceObj = await repository.AddResourceAsync(resource);
                if (resourceObj == null)
                {
                    return BadRequest("Resource submitted is invalid");
                }
                else
                {
                    return Created("/resource", resourceObj);
                }
            }
            return BadRequest();
            // return Ok(Resources);
        }

        /// <summary>
        /// Updates a <paramref name="resource"/> into the database if it
        /// exists
        /// </summary>
        /// <param name="resource">Resource to be added. </param>
        /// <param name="resourceId">resourceId of object which needs to be updated. </param>
        /// <response code="201">Returns the newly updated resource</response>
        /// <response code="400">If the resource does not exists or modelstate is invalid </response> 
        // PUT SME/
        [HttpPut("{resourceId}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PutAsync(string resourceId, [FromBody] Resource resource)
        {
            // resource.ResourceId = resourceId;
            if (ModelState.IsValid)
            {
                var resourceObj = await repository.UpdateResourceAsync(resource);
                if (resourceObj == null)
                {
                    return NotFound("Resource was not found or You didn't include it's ID");
                }
                else
                {
                    return Created("/resource", resourceObj);
                }
            }
            return BadRequest();
        }

        /// <summary>
        /// Deleted a Resource from the Database
        /// </summary>
        /// <response code="200">Deleted a Resource </response>
        /// <response code="404">Resource not found</response>
        // GET Resource/
        [HttpDelete("{resourceId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteResourcesAsync(string resourceId)
        {
            var hasDeleted = await repository.DeleteResourceAsync(resourceId);
            if (hasDeleted)
            {
                return Ok("Resource has been deleted");
            }
            else
            {
                return NotFound();
            }
        }
    }
}