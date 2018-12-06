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
        /// <response code="200">Returns all questions inside a resource </response>
        /// <response code="404">questions not found </response>
        // GET Resource/
        [HttpGet("{resourceId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetQuestionAsync(string resourceId)
        {
            var question = await repository.GetQuestionsByResourceAsync(resourceId);
            if (question == null)
            {
                return NotFound();
            }
            return Ok(question);
        }
    }
}