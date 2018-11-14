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
    public class QuestionController : ControllerBase
    {
        // dependency injection for the repository interface
        // which is responsible for business logic for interacting
        // with the database
        private IQuestionRepository repository;

        public QuestionController(IQuestionRepository repository)
        {
            this.repository = repository;
        }
        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult GetQuestions()
        {
            var resources = repository.GetQuestions();
            if (resources == null)
            {
                resources = new List<Question>();
            }
            return Ok(resources);
        }
        /// <summary>
        /// Gets questions from the database according to concept and technology
        /// </summary>
        /// <param name="technology">Name of technology you want questions of</param>
        /// <param name="concept">Name of concept you want questions of</param>
        /// <response code="200">Returns the question</response>
        /// <response code="404">If the question doesn't exist for the given specs</response> 
        [HttpGet("{technology}/{concept}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult GetQuestionsByConcept_Tech(string technology, string concept)
        {
            var resources = repository.GetQuestionsByConceptOfATech(technology,concept);
            if (resources == null)
            {
                return NotFound();
            }
            return Ok(resources);
        }

        /// <summary>
        /// Posts a <paramref name="question"/> into the database
        /// </summary>
        /// <param name="question"> Object of type question which needs to be posted
        /// to the database </param>
        /// <response code="201">Returns the newly created question</response>
        /// <response code="400">If the question already exists or modelstate is invalid </response> 
        // POST SME/
        [HttpPost()]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult Post([FromBody] Question question)
        {
            if (ModelState.IsValid)
            {
                var questionObj = repository.AddQuestion(question);
                if (questionObj == null)
                {
                    return BadRequest("Question submitted is invalid");
                }
                else
                {
                    return Created("/question", questionObj);
                }
            }
            return BadRequest();
        }

        /// <summary>
        /// Updates a <paramref name="question"/> into the database if it exists
        /// </summary>
        /// <param name="question">Question to be added. </param>
        /// <param name="QuestionId">QuestionId of object which needs to be updated. </param>
        /// <response code="201">Returns the newly updated Question</response>
        /// <response code="400">If the Question does not exists or modelstate is invalid </response> 
        // PUT SME/
        [HttpPut("{QuestionId}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult Put(string QuestionId, [FromBody] Question question)
        {
            if (ModelState.IsValid)
            {
                var questionObj = repository.UpdateQuestion(question);
                if (questionObj == null)
                {
                    return NotFound("Question was not found or You didn't include it's ID");
                }
                else
                {
                    return Created("/question", questionObj);
                }
            }
            return BadRequest();
        }
    }
}