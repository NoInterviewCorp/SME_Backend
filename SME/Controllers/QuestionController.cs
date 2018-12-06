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
        /// Retrieves all questions from the Database by its name
        /// </summary>
        /// <response code="200">Returns all questions inside a question </response>
        /// <response code="404">questions not found </response>
        // GET question/
        [HttpGet("{questionId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetQuestionAsync(string questionId)
        {
            var question = await repository.GetQuestionsByResourceAsync(questionId);
            if (question == null)
            {
                return NotFound();
            }
            return Ok(question);
        }

        /// <summary>
        /// Posts a <paramref name="questions"/> into the database
        /// </summary>
        /// <param name="questions"> Object of type question which needs to be posted
        /// to the database </param>
        /// <param name="questionId"> question id of the question to which question which needs to be posted
        /// to the database </param>
        /// <response code="201">Returns the newly created question</response>
        /// <response code="400">If the question already exists or modelstate is invalid </response> 
        // POST SME/
        [HttpPost("{questionId}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PostAsync(string questionId, [FromBody] List<Question> questions)
        {
            if (ModelState.IsValid)
            {
                var questionObj = await repository.AddQuestionsAsync(questions, questionId);
                if (questionObj == null)
                {
                    return BadRequest("question submitted is invalid");
                }
                else
                {
                    return Created("/question", questionObj);
                }
            }
            return BadRequest();
        }

        /// <summary>
        /// Updates a <paramref name="question"/> into the database if it
        /// exists
        /// </summary>
        /// <param name="question">question to be added. </param>
        /// <param name="questionId">questionId of object which needs to be updated. </param>
        /// <response code="201">Returns the newly updated question</response>
        /// <response code="400">If the question does not exists or modelstate is invalid </response> 
        // PUT SME/
        [HttpPut("{questionId}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PutAsync(string questionId, [FromBody] Question question)
        {
            // question.questionId = questionId;
            if (ModelState.IsValid)
            {
                var questionObj = await repository.UpdateQuestionAsync(question);
                if (questionObj == null)
                {
                    return NotFound("question was not found or You didn't include it's ID");
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