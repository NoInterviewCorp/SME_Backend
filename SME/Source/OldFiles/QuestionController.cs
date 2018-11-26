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
        /// <summary>
        /// Gets questions from the database
        /// </summary>
        /// <response code="200">Returns all questions</response>
        /// <response code="404">If any question doesn't exist</response>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetQuestionsAsync()
        {
            var Questions = await repository.GetQuestionsAsync();
            if (Questions.Count == 0)
            {
                return NotFound();
            }
            return Ok(Questions);
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
        public async Task<IActionResult> GetQuestionsByConcept_TechAsync(string technology, string concept)
        {
            var Questions = await repository.GetQuestionsByConceptOfATechAsync(technology,concept);
            if (Questions == null)
            {
                return NotFound();
            }
            return Ok(Questions);
        }

        /// <summary>
        /// Posts many <paramref name="questions"/> into the database
        /// </summary>
        /// <param name="questions"> Object of type question which needs to be posted
        /// to the database </param>
        /// <response code="201">Returns the newly created question</response>
        /// <response code="400">If the question already exists or modelstate is invalid </response> 
        // POST SME/
        [HttpPost()]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PostAsync([FromBody] List<Question> questions)
        {
            if (ModelState.IsValid)
            {
                var questionObj = await repository.AddQuestionsAsync(questions);
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
        public async Task<IActionResult> PutAsync(string QuestionId, [FromBody] Question question)
        {
            if (ModelState.IsValid)
            {
                var questionObj = await repository.UpdateQuestionAsync(question);
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

        /// <summary>
        /// Deleted a Question from the Database
        /// </summary>
        /// <response code="200">Deleted a Question </response>
        /// <response code="404">Question not found</response>
        // GET Question/
        [HttpDelete("{QuestionId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteQuestionsAsync(string QuestionId)
        {
            var hasDeleted = await repository.DeleteQuestionByIdAsync(QuestionId);
            if (hasDeleted)
            {
                return Ok("Question has been deleted");
            }
            else
            {
                return NotFound();
            }
        }
    }
}