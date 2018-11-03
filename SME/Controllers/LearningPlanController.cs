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
        /// Retrieves All Learning Plans created by the user from the Database
        /// </summary>
        /// <response code="200">Returns all Learning Plans by a particular user through his username </response>
        // GET Resource/
        [HttpGet("{username}")]
        [ProducesResponseType(200)]
        public IActionResult GetUserLearningPlans(string username)
        {
            var resources = repository.GetLearningPlansByUserName(username);
            if (resources == null)
            {
                return Ok("There are no Learning plans. You can create your own Learning plan");
            }
            return Ok(resources);
        }

        /// <summary>
        /// Retrieves a Learning Plans by its id from the Database
        /// </summary>
        /// <response code="200">Returns a Learning Plans by id </response>
        // GET Resource/
        [HttpGet("id/{learningPlanId}")]
        [ProducesResponseType(200)]
        public IActionResult GetUserLearningPlanById(string learningPlanId)
        {
            var resource = repository.GetLearningPlanById(learningPlanId);
            if (resource == null)
            {
                return Ok("There are no Learning plans. You can create your own Learning plan");
            }
            return Ok(resource);
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
    }
}