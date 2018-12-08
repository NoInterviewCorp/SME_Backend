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
            Console.WriteLine("RPC to get LP Info");
            var learningPlanIds = LearningPlans.Select(lp => lp.LearningPlanId).ToList();
            Console.WriteLine(learningPlanIds.Count);
            var lpInfos = mQConnection.GetLearningPlanInfo(learningPlanIds);

            var result = AddInfoToPlans(lpInfos, LearningPlans);
            return Ok(result);
        }

        private List<LearningPlan> AddInfoToPlans(List<LearningPlanInfo> infos, List<LearningPlan> lps)
        {
            var result = new List<LearningPlan>();
            foreach (var info in infos)
            {
                var plan = lps.FirstOrDefault(lp => lp.LearningPlanId == info.LearningPlanId);
                if (plan != null)
                {
                    plan.AverageRating = info.AverageRating;
                    plan.TotalSubscribers = info.TotalSubscribers;
                    result.Add(plan);
                }
            }
            return result;
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
                    var learningPlanIds = LearningPlans.Select(lp => lp.LearningPlanId).ToList();
                    var lpInfos = mQConnection.GetLearningPlanInfo(learningPlanIds);
                    var result = AddInfoToPlans(lpInfos, LearningPlans);
                    return Ok(result);
                case "id":
                    var LearningPlan = await repository.GetLearningPlanByIdAsync(text);
                    if (LearningPlan == null)
                    {
                        return Ok("There are no Learning plans. You can create your own Learning plan");
                    }
                    var lpInfos2 = mQConnection.GetLearningPlanInfo(new List<string> { LearningPlan.LearningPlanId });
                    LearningPlan.AverageRating = lpInfos2[0].AverageRating;
                    LearningPlan.TotalSubscribers = lpInfos2[0].TotalSubscribers;
                    return Ok(LearningPlan);
                case "tech":
                    var LearningPlansObj = await repository.GetLearningPlansByTechnologyAsync(text);
                    if (LearningPlansObj == null)
                    {
                        return Ok("There are no Learning plans. You can create your own Learning plan");
                    }
                    var learningPlanIds3 = LearningPlansObj.Select(lp => lp.LearningPlanId).ToList();
                    var lpInfos3 = mQConnection.GetLearningPlanInfo(learningPlanIds3);
                    var result3 = AddInfoToPlans(lpInfos3, LearningPlansObj);
                    return Ok(result3);
                case "popular":
                    var resp = mQConnection.GetPopularPlans(text);
                    var popularPlans = await repository.GetLearningPlansByInfos(resp);
                    var resultPopular = AddInfoToPlans(resp, popularPlans);
                    return Ok(resultPopular);
                case "subscriptions":
                    var subs = mQConnection.GetSubscriptions(text);
                    var subscriptions = await repository.GetLearningPlansByInfos(subs);
                    var resultSubs = AddInfoToPlans(subs, subscriptions);
                    return Ok(resultSubs);
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
                try
                {
                    Console.WriteLine(learningPlan.Resources[0].ResourceId);
                    var replaceLearningPlanResult = await repository.AddLearningPlanAsync(learningPlan);
                    if (replaceLearningPlanResult.IsAcknowledged)
                    {
                        Console.WriteLine(replaceLearningPlanResult.UpsertedId);
                        learningPlan.LearningPlanId = replaceLearningPlanResult.UpsertedId.ToString();
                        var lpWrapper = new LearningPlanWrapper(learningPlan);
                        // var objectWrapper = new ObjectWrapper(MessageType.IsLearningPlan,lpWrapper as Object);
                        var body = ObjectSerialize.Serialize(lpWrapper);
                        mQConnection.Model.BasicPublish(
                            exchange: mQConnection.ExchangeName,
                            routingKey: "Models.LearningPlan",
                            basicProperties: null,
                            body: body
                        );
                        Console.WriteLine(" [x] Sent Learning Plan with the name -> {0}", learningPlan.Name);
                        return Created("/learningplan/" + learningPlan.LearningPlanId, learningPlan);
                    }
                }
                catch (Exception e)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(e.Message);
                    sb.Append("\n");
                    sb.Append(e.StackTrace);
                    sb.Append("\n");
                    sb.Append(e.InnerException);
                    Console.WriteLine("--------------------------------------------------------------");
                    Console.WriteLine(sb);
                    Console.WriteLine("--------------------------------------------------------------");
                    return BadRequest(e);
                }
            }
            return BadRequest(ModelState);
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
                try
                {
                    await repository.UpdateLearningPlanAsync(learningPlan);
                    return Created("/learningplan/" + learningPlan.LearningPlanId, learningPlan);
                }
                catch (Exception e)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(e.Message);
                    sb.Append("\n");
                    sb.Append(e.StackTrace);
                    sb.Append("\n");
                    sb.Append(e.InnerException);
                    Console.WriteLine("--------------------------------------------------------------");
                    Console.WriteLine(sb);
                    Console.WriteLine("--------------------------------------------------------------");
                    return BadRequest(e);
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
                return Ok($"LearningPlan : {learningPlanId} has been deleted");
            }
            else
            {
                return NotFound();
            }
        }
    }
}
