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
        private IDatabaseRepository repository;

        public ResourceController(IDatabaseRepository repository)
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
        public IActionResult GetResources()
        {
            var resources = repository.GetResources();
            if (resources == null)
            {
                resources = new List<Resource>();
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
        public IActionResult Post([FromBody] Resource resource)
        {
            if (ModelState.IsValid)
            {
                var resourceObj = repository.AddResource(resource);
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
        }       
    }
}

//         /// <summary>
//         /// Retrieves all database content.
//         /// Used to transfer SME data to Learner Evaluation Module
//         /// </summary>
//         /// <response code="200">Returns the database</response>
//         // GET SME/
//         [Route("/all")]
//         [HttpGet]
//         [ProducesResponseType(200)]
//         public IActionResult GetAll()
//         {
//             var technologies = repository.GetAllData();
//             if (technologies == null)
//             {
//                 technologies = new List<Technology>();
//             }
//             return Ok(technologies);
//         }

//         /// <summary>
//         /// Retrieves All technologies with their topics from
//         /// the Database. Does not include Questions        
//         /// </summary>
//         /// <response code="200">Returns Technologies with theirTopics</response>
//         // GET SME/
//         [HttpGet]
//         [ProducesResponseType(200)]
//         public IActionResult Get()
//         {
//             var technologies = repository.GetAllTechnologies();
//             if (technologies == null)
//             {
//                 technologies = new List<Technology>();
//             }
//             return Ok(technologies);
//         }

//         /// <summary>
//         /// Retrieves either all topics in a <paramref name="technology"/>
//         /// or Retrieves all questions inside a given topics. With <paramref name="hasPublished"/> 
//         /// one can retrieve saved or published questions
//         /// </summary>
//         /// <response code="200">Returns Questions according to the request</response>
//         /// <response code="404">If the item was not found</response> 
//         // GET SME/technology
//         [HttpGet("{technology}")]
//         [ProducesResponseType(200)]
//         [ProducesResponseType(404)]
//         public IActionResult Get(string technology, [FromQuery] string topic, [FromQuery] bool hasPublished)
//         {
//             // if the request doesn't contain any query parameters we give all topics
//             // in a particular technology
//             // GET SME/Angular
//             if (topic == null)
//             {
//                 var topics = repository.GetAllTopicsInATechnology(technology);
//                 if (topics == null)
//                 {
//                     return NotFound();
//                 }
//                 return Ok(topics);
//             }
//             // else we will respond with all the questions containing in a topic
//             // filtering according to if they're published or not
//             // GET SME/Angular?topic=Routing&hasPublished=true
//             else
//             {
//                 var questions = repository.GetAllQuestionsFromTopic(technology, topic, hasPublished);
//                 if (questions == null)
//                 {
//                     return NotFound();
//                 }
//                 return Ok(questions);
//             }
//         }

//         /// <summary>
//         /// Posts a <paramref name="question"/> into the database
//         /// </summary>
//         /// <response code="201">Returns the newly created question</response>
//         /// <response code="400">If the question already exists or modelstate is invalid </response> 
//         // POST SME/Angular
//         [HttpPost("{technology}")]
//         [ProducesResponseType(201)]
//         [ProducesResponseType(400)]
//         public IActionResult Post([FromBody] Question question)
//         {
//             if (ModelState.IsValid)
//             {
//                 var questionObj = repository.PostToTopic(question);
//                 if (questionObj == null)
//                 {
//                     return BadRequest("Input value is invalid");
//                 }
//                 else
//                 {
//                     return Created("sme", questionObj);
//                 }
//             }
//             return BadRequest();
//         }

//         /// <summary>
//         /// Posts all the questions from the excel file into the database
//         /// </summary>
//         /// <response code="201">Returns the newly created question</response>
//         /// <response code="400">If the question already exists or modelstate is invalid </response> 
//         // POST SME/Angular
//         [HttpPost()]
//         [Route("/SME/Excel")]
//         [ProducesResponseType(201)]
//         [ProducesResponseType(400)]
//         public IActionResult Post([FromBody] string path)
//         {
//             if (ModelState.IsValid)
//             {
//                 var questionObj = repository.AddQuestionsFromExcel();
//                 if (questionObj == null)
//                 {
//                     return BadRequest("Input value is invalid");
//                 }
//                 else
//                 {
//                     return Created("sme", questionObj);
//                 }
//             }
//             return BadRequest();
//         }


//         /// <summary>
//         /// Updates a <paramref name="technology"/> into the database if it
//         /// exists
//         /// </summary>
//         /// <response code="201">Returns the newly updated <paramref name="technology"/></response>
//         /// <response code="400">If the <paramref name="technology"/> doesn not exists or modelstate is invalid </response> 
//         // PUT SME/
//         [HttpPut()]
//         [ProducesResponseType(201)]
//         [ProducesResponseType(400)]
//         public IActionResult Put([FromBody] Technology technology)
//         {
//             if (ModelState.IsValid)
//             {
//                 var technologyObj = repository.UpdateTechnology(technology);
//                 if (technologyObj == null)
//                 {
//                     return NotFound(technology.Name + " was not found or You didn't include it's ID");
//                 }
//                 else
//                 {
//                     return Created("/sme",technologyObj);
//                 }
//             }
//             return BadRequest();
//         }
//         /// <summary>
//         /// Updates a <paramref name="question"/> into the database if it
//         /// exists
//         /// </summary>
//         /// <response code="201">Returns the newly created <paramref name="question"/></response>
//         /// <response code="400">If the <paramref name="question"/> does not exists or modelstate is invalid </response> 
//         // PUT SME/Angular
//         [HttpPut("{technology}")]
//         [ProducesResponseType(201)]
//         [ProducesResponseType(400)]
//         public IActionResult Put([FromBody] Question question)
//         {
//             if (ModelState.IsValid)
//             {
//                 var questionObj = repository.UpdateQuestion(question);
//                 if (questionObj == null)
//                 {
//                     return NotFound("Question with id : " + question.QuestionId + " was not found");
//                 }
//                 else
//                 {
//                     return Ok("Question with id : " + question.QuestionId + " has been updated");
//                 }
//             }
//             return BadRequest();
//         }
//         /// <summary>
//         /// Deletes a <paramref name="technology"/> or a question
//         /// using it's id
//         /// </summary>
//         /// <response code="200">On a successful deletion</response>
//         /// <response code="404">If the item does not exists </response> 
//         // DELETE SME/Angular
//         // or
//         // DELETE SME/Angular?questionid=1
//         [HttpDelete("{technology}")]
//         [ProducesResponseType(200)]
//         [ProducesResponseType(404)]
//         public IActionResult Delete(string technology,[FromQuery] int questionId)
//         {
//             bool hasDeleted;
//             if (questionId != 0)
//             {
//                 hasDeleted = repository.DeleteQuestionById(questionId);
//             }
//             else
//             {
//                 hasDeleted = repository.DeleteTechnology(technology);
//             }
//             if (!hasDeleted)
//             {
//                 return NotFound("Deletion failed. Your input values was invalid.");
//             }
//             else
//             {
//                 return Ok("Question has been successfully deleted");
//             }
//         }
//     }
// }
