using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SME.Models;
using SME.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace SME.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SMEController : ControllerBase
    {
        private IDatabaseRepository repository;

        SMEController(IDatabaseRepository repository)
        {
            this.repository = repository;
        }

        // GET api/SME
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

        // GET api/SME/
        [HttpGet("{technology}")]
        public IActionResult Get(string technolgy, [FromQuery] string topic, [FromQuery] int bloomAsInt)
        {
            // if the request doesn't contain any query parameters we give all topics
            // in a particular technology
            if (topic == "" && bloomAsInt == 0)
            {
                var topics = repository.GetAllTopicsInATechnology(technolgy);
                if (topics == null)
                {
                    topics = new List<Topic>();
                }
                return Ok(topics);
            }
            // else we will respond with all the questions containing in a particular
            // topic of a particular bloomLevel
            else
            {
                // convert the bloomlevel from integer to enum form
                BloomTaxonomy bloomLevel = (BloomTaxonomy)bloomAsInt;
                var questions = repository.GetAllQuestionsFromTopic(technolgy, topic, bloomLevel);
                if (questions == null)
                {
                    questions = new List<Question>();
                }
                return Ok(questions);
            }
        }

        // POST api/SME
        [HttpPost]
        public IActionResult Post([FromBody] Technology technology)
        {
            var technologyObj = repository.PostToTechnology(technology);
            if (technologyObj == null)
            {
                return BadRequest("Input value is invalid");
            }
            else
            {
                return Created("api/tech",technologyObj);
            }
        }

        // PUT api/SME/5
        [HttpPut("{techName}")]
        public IActionResult Put(string techName, [FromBody] Technology technology)
        {
            var technologyObj = repository.UpdateQuestions(techName, technology);
            if (technologyObj == null)
            {
                return NotFound();
            }
            else
            {
                return Ok();
            }
        }

        // DELETE api/SME/5
        [HttpDelete("{techName}")]
        public IActionResult Delete(string techName, [FromQuery] string topicName, [FromQuery] int questionId)
        {
            var hasDeleted = repository.DeleteQuestionById(techName, topicName, questionId);
            if(!hasDeleted){
                return NotFound("Deletion failed. Your input values was invalid.");
            }
            else{
                return Ok("Question has been successfully deleted");
            }
        }
    }
}
