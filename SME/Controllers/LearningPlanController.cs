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
    public class LearningPlanController : ControllerBase
    {
        // dependency injection for the repository interface
        // which is responsible for business logic for interacting
        // with the database
        private IDatabaseRepository repository;

        public LearningPlanController(IDatabaseRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Retrieves All Learning Plans from the Database
        /// </summary>
        /// <response code="200">Returns all Learning Plans </response>
        // GET Resource/
        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult GetLearningPlans()
        {
            var resources = repository.GetLearningPlans();
            if (resources == null)
            {
                return Ok("There are no Learning plans. You can create your own Learning plan");
            }
            return Ok(resources);
        }

        /// <summary>
        /// Retrieves Learning Plan/s by username/id/technology from the Database
        /// <param name="text">Could be an username or id or technology</param>
        /// <param name="type">Possible values are "id", "username", "tech"</param>
        /// </summary>
        /// <response code="200">Returns Learning Plan/s according to <paramref name="type"/> given </response>
        /// <response code="400">Bad Request as type maybe invalid </response>
        // GET Resource/
        [HttpGet("{text}")]
        [ProducesResponseType(200)]
        public IActionResult GetUserLearningPlans(string text, [FromQuery] string type)
        {
            switch (type.ToLower())
            {
                case "username":
                    var resources = repository.GetLearningPlansByUserName(text);
                    if (resources == null)
                    {
                        return Ok("There are no Learning plans. You can create your own Learning plan");
                    }
                    return Ok(resources);
                case "id":
                    var resource = repository.GetLearningPlanById(text);
                    if (resource == null)
                    {
                        return Ok("There are no Learning plans. You can create your own Learning plan");
                    }
                    return Ok(resource);
                case "tech":
                    var resourcesObj = repository.GetLearningPlansByTechnology(text);
                    if (resourcesObj == null)
                    {
                        return Ok("There are no Learning plans. You can create your own Learning plan");
                    }
                    return Ok(resourcesObj);
                default:
                    return BadRequest();
            }

        }

        /// <summary>
        /// Posts a <paramref name="learningPlan"/> into the database
        /// </summary>
        /// <param name="learningPlan"> Object of type concept which needs to be posted
        /// to the database </param>
        /// <response code="201">Returns the newly created concept</response>
        /// <response code="400">If the concept already exists or modelstate is invalid </response> 
        // POST concept/
        [HttpPost()]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult Post([FromBody] LearningPlan learningPlan)
        {
            if (ModelState.IsValid)
            {
                var learningplanObj = repository.AddLearningPlan(learningPlan);
                if (learningplanObj == null)
                {
                    return BadRequest("Learning Plan submitted is invalid");
                }
                else
                {
                    return Created("concept", learningplanObj);
                }
            }
            return BadRequest();
        }

        /// <summary>
        /// Updates a <paramref name="learningPlan"/> into the database if it exists
        /// </summary>
        /// <param name="learningPlan">LearningPlan to be added. </param>
        /// <param name="learningPlanId">learningPlanId of object which needs to be updated. </param>
        /// <response code="201">Returns the newly updated LearningPlan</response>
        /// <response code="400">If the LearningPlan does not exists or modelstate is invalid </response> 
        // PUT SME/
        [HttpPut("{learningPlanId}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult Put(string learningPlanId, [FromBody] LearningPlan learningPlan)
        {
            if (ModelState.IsValid)
            {
                var learningPlanObj = repository.UpdateLearningPlan(learningPlan);
                if (learningPlanObj == null)
                {
                    return NotFound(learningPlan.Name + " was not found or You didn't include it's ID");
                }
                else
                {
                    return Created("/sme", learningPlanObj);
                }
            }
            return BadRequest();
        }
    }
}