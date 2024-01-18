using Microsoft.AspNetCore.Mvc;

using Snipper_Snippet_API.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Snipper_Snippet_API.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class SnippetController : ControllerBase
    {
        List<Snippet> snippets = new List<Snippet>{ new Snippet (Id: 0, Langauge: "Python", Code: "print('Hello, World!')" ) };
        // GET: api/<SnippetController>
        [HttpGet]
        public ActionResult<List<Snippet>> GetAll()
        {
            return Ok(snippets);
        }

        // GET api/<SnippetController>/5
        [HttpGet("{id}")]
        public ActionResult<Snippet> Get(int id)
        {
            var snippet = snippets.FirstOrDefault(s => s.Id == id);
            if(snippet == null)
            {
                return BadRequest("Not Found");
            }
            return Ok(snippet);
        }

        // POST api/<SnippetController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<SnippetController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SnippetController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }
    }
}
