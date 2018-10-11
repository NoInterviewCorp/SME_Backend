using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SME.Models;
using Microsoft.AspNetCore.Mvc;

namespace SME.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SMEController : ControllerBase
    {
        // GET api/SME
        [HttpGet]
        public IActionResult Get()
        {
            BloomTaxonomy bloom = (BloomTaxonomy)6;
            return Ok(bloom.ToString());
        }

        // // GET api/SME/
        // [HttpGet("{techName}")]
        // public IActionResult Get(string techName, [FromQuery] int bloomLevel)
        // {
        //     return "value";
        // }

        // // POST api/SME
        // [HttpPost]
        // public IActionResult Post([FromBody] string value)
        // {
        // }

        // // PUT api/SME/5
        // [HttpPut("{id}")]
        // public IActionResult Put(int id, [FromBody] string value)
        // {
        // }

        // // DELETE api/SME/5
        // [HttpDelete("{id}")]
        // public IActionResult Delete(int id)
        // {
        // }
    }
}
