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
    public class SMEController : ControllerBase
    {
        private IDatabaseRepository repository;

        public SMEController(IDatabaseRepository repository)
        {
            this.repository = repository;
        }

        // GET SME
        [HttpGet]
        public IActionResult Get()
        {
            var technologies = repository.GetAllTechnologies();
            if (technologies == null)
            {
                technologies = new List<Technology>();
            }
            return Ok(technologies);
        }

        // GET SME
        [HttpGet("{technology}")]
        public IActionResult Get(string technology, [FromQuery] string topic, [FromQuery] int bloomAsInt)
        {
            // if the request doesn't contain any query parameters we give all topics
            // in a particular technology
            if (topic == null && bloomAsInt == 0)
            {
                var topics = repository.GetAllTopicsInATechnology(technology);
                if (topics == null)
                {
                    return NotFound();
                }
                return Ok(topics);
            }
            // else we will respond with all the questions containing in a particular
            // topic of a particular bloomLevel
            else
            {
                // convert the bloomlevel from integer to enum form
                BloomTaxonomy bloomLevel = (BloomTaxonomy)bloomAsInt;
                var questions = repository.GetAllQuestionsFromTopic(technology, topic, bloomLevel);
                if (questions == null)
                {
                    return NotFound();
                }
                return Ok(questions);
            }
        }

        // POST SME
        [HttpPost()]
        public IActionResult Post([FromBody] Technology technology)
        {
            if (ModelState.IsValid)
            {
                var technologyObj = repository.PostToTechnology(technology);
                if (technologyObj == null)
                {
                    return BadRequest("Input value is invalid");
                }
                else
                {
                    return Created("sme", technologyObj);
                }
            }
            return BadRequest();
        }

        // POST SME/Angular
        [HttpPost("{technology}")]
        public IActionResult Post([FromBody] Question question)
        {
            if (ModelState.IsValid)
            {
                var questionObj = repository.PostToTopic(question);
                if (questionObj == null)
                {
                    return BadRequest("Input value is invalid");
                }
                else
                {
                    return Created("sme", questionObj);
                }
            }
            return BadRequest();
        }

        // PUT SME/5
        [HttpPut()]
        public IActionResult Put([FromBody] Technology technology)
        {
            if (ModelState.IsValid)
            {
                var technologyObj = repository.UpdateTechnology(technology);
                if (technologyObj == null)
                {
                    return NotFound(technology.Name + " was not found or You didn't include it's ID");
                }
                else
                {
                    return Created("/sme",technologyObj);
                }
            }
            return BadRequest();
        }

        // PUT SME/Angular/5
        [HttpPut("{technology}")]
        public IActionResult Put([FromBody] Question question)
        {
            if (ModelState.IsValid)
            {
                var questionObj = repository.UpdateQuestions(question);
                if (questionObj == null)
                {
                    return NotFound("Question with id : " + question.QuestionId + " was not found");
                }
                else
                {
                    return Ok("Question with id : " + question.QuestionId + " has been updated");
                }
            }
            return BadRequest();
        }

        // DELETE SME/5
        [HttpDelete("{technology}")]
        public IActionResult Delete(string technology,[FromQuery] int questionId)
        {
            bool hasDeleted;
            if (questionId != 0)
            {
                hasDeleted = repository.DeleteQuestionById(questionId);
            }
            else
            {
                hasDeleted = repository.DeleteTechnology(technology);
            }
            if (!hasDeleted)
            {
                return NotFound("Deletion failed. Your input values was invalid.");
            }
            else
            {
                return Ok("Question has been successfully deleted");
            }
        }
    }
}
