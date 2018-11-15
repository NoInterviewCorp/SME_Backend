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

        // /// <summary>
        // /// Retrieves All Resources from the Database
        // /// </summary>
        // /// <response code="200">Returns all Resources </response>
        // // GET Resource/
        // [HttpGet]
        // [ProducesResponseType(200)]
        // public IActionResult GetResources()
        // {
        //     var resources = repository.GetResourcesAsync();
        //     if (resources == null)
        //     {
        //         resources = new List<Resource>();
        //     }
        //     return Ok(resources);
        // }

        // /// <summary>
        // /// Retrieves a resource from the Database which belongs to technology i.e type=>"tech"
        // /// or by a resource's link i.e type=>"link"
        // /// </summary>
        // /// <param name="text">could be technology/resourcelink for which we need all resources</param>
        // /// <param name="type">possible values are "tech" and "link"</param>
        // /// <response code="200">Returns all resource </response>
        // /// <response code="404">Resource not found </response>
        // /// <response code="400">Invalid "type" from query.null Should be either tech or link</response>
        // // GET Resource/
        // [HttpGet("{text}")]
        // [ProducesResponseType(200)]
        // [ProducesResponseType(404)]
        // [ProducesResponseType(400)]
        // public IActionResult GetResource(string text,[FromQuery] string type)
        // {
        //     switch(type.ToLower()){
        //         case "tech":
        //             var resources = repository.GetResourceByTechnologyAsync(text);
        //             if (resources == null)
        //             {
        //                 return NotFound();
        //             }
        //             return Ok(resources);
        //         case "link":
        //             var resource = repository.GetResourceByLinkAsync(text);
        //             if (resource == null)
        //             {
        //                 return NotFound();
        //             }
        //             return Ok(resource);
        //         default:
        //             return BadRequest();
        //     }
        // }

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
            // return Ok(concepts);
        }

        // /// <summary>
        // /// Updates a <paramref name="resource"/> into the database if it
        // /// exists
        // /// </summary>
        // /// <param name="resource">Resource to be added. </param>
        // /// <param name="resourceId">resourceId of object which needs to be updated. </param>
        // /// <response code="201">Returns the newly updated resource</response>
        // /// <response code="400">If the resource does not exists or modelstate is invalid </response> 
        // // PUT SME/
        // [HttpPut("{resourceId}")]
        // [ProducesResponseType(201)]
        // [ProducesResponseType(400)]
        // public IActionResult Put(string resourceId,[FromBody] Resource resource)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         var resourceObj = repository.UpdateResourceAsync(resource);
        //         if (resourceObj == null)
        //         {
        //             return NotFound( "Resource was not found or You didn't include it's ID");
        //         }
        //         else
        //         {
        //             return Created("/resource", resourceObj);
        //         }
        //     }
        //     return BadRequest();
        // }
    }
}