using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SME.Models;
using SME.Persistence;
using Microsoft.AspNetCore.Mvc;
using SME.Services;
using System.Text;
using RabbitMQ.Client;

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

        private RabbitMQConnection mQConnection;

        public LearningPlanController(ILearningPlanRepository repository, RabbitMQConnection mQConnection)
        {
            this.mQConnection = mQConnection;
            this.repository = repository;
        }

        /// <summary>
        /// Retrieves All Learning Plans from the Database
        /// </summary>
        /// <response code="200">Returns all Learning Plans </response>
        // GET LearningPlan/
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetLearningPlans()
        {
            var LearningPlans = await repository.GetLearningPlansAsync();
            if (LearningPlans == null)
            {
                return NotFound("There are no Learning plans. You can create your own Learning plan");
            }
            return Ok(LearningPlans);
        }

        /// <summary>
        /// Retrieves Learning Plan/s by username/id/technology from the Database
        /// <param name="text">Could be an username or id or technology</param>
        /// <param name="type">Possible values are "id", "username", "tech"</param>
        /// </summary>
        /// <response code="200">Returns Learning Plan/s according to <paramref name="type"/> given </response>
        /// <response code="400">Bad Request as type maybe invalid </response>
        // GET LearningPlan/
        [HttpGet("{text}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetUserLearningPlansAsync(string text, [FromQuery] string type)
        {
            switch (type.ToLower())
            {
                case "username":
                    var LearningPlans = await repository.GetLearningPlansByUserNameAsync(text);
                    if (LearningPlans == null)
                    {
                        return Ok("There are no Learning plans. You can create your own Learning plan");
                    }
                    return Ok(LearningPlans);
                case "id":
                    var LearningPlan = await repository.GetLearningPlanByIdAsync(text);
                    if (LearningPlan == null)
                    {
                        return Ok("There are no Learning plans. You can create your own Learning plan");
                    }
                    return Ok(LearningPlan);
                case "tech":
                    var LearningPlansObj = await repository.GetLearningPlansByTechnologyAsync(text);
                    if (LearningPlansObj == null)
                    {
                        return Ok("There are no Learning plans. You can create your own Learning plan");
                    }
                    return Ok(LearningPlansObj);
                // case "saved":
                //     var savedLearningPlans = await repository.GetSavedLearningPlansOfUser(text);
                //     if (savedLearningPlans == null)
                //     {
                //         return Ok("There are no Learning plans. You can create your own Learning plan");
                //     }
                //     return Ok(savedLearningPlans);
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
                    // if (learningPlan.HasPublished)
                    // {
                        var lpWrapper = new LearningPlanWrapper(learningPlan);
                        // var objectWrapper = new ObjectWrapper(MessageType.IsLearningPlan,lpWrapper as Object);
                        var body = ObjectSerialize.Serialize(lpWrapper);
                        mQConnection.Model.BasicPublish(
                            exchange: mQConnection.ExchangeName,
                            routingKey: "Models.LearningPlan",
                            basicProperties: null,
                            body: body
                        );
                        Console.WriteLine(" [x] Sent {0}", lpWrapper.LearningPlanId);
                    // }
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

        /// <summary>
        /// Deleted a LearningPlan from the Database
        /// </summary>
        /// <response code="200">Deleted a LearningPlan </response>
        /// <response code="404">LearningPlan not found</response>
        // GET LearningPlan/
        [HttpDelete("{learningPlanId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteLearningPlanAsync(string learningPlanId)
        {
            var hasDeleted = await repository.DeleteLearningPlanAsync(learningPlanId);
            if (hasDeleted)
            {
                return Ok("LearningPlan has been deleted");
            }
            else
            {
                return NotFound();
            }
        }
    }
}