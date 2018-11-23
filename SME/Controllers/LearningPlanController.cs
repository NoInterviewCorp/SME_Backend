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
        private ILearningPlanRepository repository;

        public LearningPlanController(ILearningPlanRepository repository)
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
        public async Task<IActionResult> GetLearningPlans()
        {
            var resources = await repository.GetLearningPlansAsync();
            if (resources == null)
            {
                return NotFound("There are no Learning plans. You can create your own Learning plan");
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
        public async Task<IActionResult> GetUserLearningPlansAsync(string text, [FromQuery] string type)
        {
            switch (type.ToLower())
            {
                case "username":
                    var resources = await repository.GetLearningPlansByUserNameAsync(text);
                    if (resources == null)
                    {
                        return Ok("There are no Learning plans. You can create your own Learning plan");
                    }
                    return Ok(resources);
                case "id":
                    var resource = await repository.GetLearningPlanByIdAsync(text);
                    if (resource == null)
                    {
                        return Ok("There are no Learning plans. You can create your own Learning plan");
                    }
                    return Ok(resource);
                case "tech":
                    var resourcesObj = await repository.GetLearningPlansByTechnologyAsync(text);
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
        public async Task<IActionResult> PostAsync([FromBody] LearningPlan learningPlan)
        {
            if (ModelState.IsValid)
            {
                var learningplanObj = await repository.AddLearningPlanAsync(learningPlan);
                if (learningplanObj == null)
                {
                    return BadRequest("Learning Plan submitted is invalid");
                }
                else
                {
                    return Created("/learningplan", learningplanObj);
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
        public async Task<IActionResult> PutAsync(string learningPlanId, [FromBody] LearningPlan learningPlan)
        {
            if (ModelState.IsValid)
            {
                var learningPlanObj = await repository.UpdateLearningPlanAsync(learningPlan);
                if (learningPlanObj == null)
                {
                    return NotFound(learningPlan.Name + " was not found or You didn't include it's ID");
                }
                else
                {
                    return Created("/learningplan", learningPlanObj);
                }
            }
            return BadRequest();
        }
    }
}