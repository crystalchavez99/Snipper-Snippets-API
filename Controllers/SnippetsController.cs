
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Snipper_Snippet_API.Models;
using Snipper_Snippet_API.Utilities;

namespace Snipper_Snippet_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SnippetsController : ControllerBase
    {
        //private readonly SnippetContext _context; used for db but need to learn how to do in memory

        public static List<Snippet> snippets = new List<Snippet>(); // list of all snippets

        public static int idTracker = snippets.Count(); // length determins id

        private readonly ILogger<SnippetsController> _logger; // regards to context but in memory

        private readonly EncryptDecrypt _encryptUtility;

            public SnippetsController(ILogger<SnippetsController> logger, EncryptDecrypt encryptUtility)
        {
            _logger = logger;
            _encryptUtility = encryptUtility; // allows usage of functions
        }

        // GET: api/Snippets
        [HttpGet]
        public ActionResult<List<Snippet>> GetSnippets()
        {
            List<Snippet> decodeSnippets = snippets.ConvertAll<Snippet>(snippet => new Snippet
            {
                Id = snippet.Id,
                Code = _encryptUtility.Decrypt(snippet.Code),
                Language = snippet.Language,
            });
            List<Snippet> allSnippets = decodeSnippets.ToList();
            return allSnippets;
          /* Applied with DB
           * if (_context.Snippets == null)
          {
              return NotFound();
          }
            return await _context.Snippets.ToListAsync();*/
        }

        // GET: api/Snippets/5
        [HttpGet("{id}")]
        public ActionResult<Snippet> GetSnippet(int id)
        {
            Snippet? findSnippet = snippets.Find(snippet => snippet.Id == id);

            if (findSnippet == null)
            {
                return NotFound("Snippet not found.");
            }

            Snippet decodedSnippet = new Snippet
            {
                Id = findSnippet.Id,
                Code = _encryptUtility.Decrypt(findSnippet.Code),
                Language = findSnippet.Language,
            };

            return decodedSnippet;
            /*if (_context.Snippets == null)
            {
                return NotFound();
            }
              var snippet = await _context.Snippets.FindAsync(id);

              if (snippet == null)
              {
                  return NotFound();
              }

              snippet.Code = Decrypt(snippet.Code);

              return snippet;*/
        }

        // PUT: api/Snippets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public ActionResult<Snippet> PutSnippet(int id, [FromBody] Snippet snippet)
        {
            Snippet? findSnippet = snippets.Find(snippet => snippet.Id == id);

            if (findSnippet == null)
            {
                return NotFound("Snippet not found.");
            }

            findSnippet.Code = _encryptUtility.Encrypt(snippet.Code);
            findSnippet.Language = snippet.Language;

            return NoContent();

            
            /*   if (id != snippet.Id)
               {
                   return BadRequest();
               }

               _context.Entry(snippet).State = EntityState.Modified;

               try
               {
                   await _context.SaveChangesAsync();
               }
               catch (DbUpdateConcurrencyException)
               {
                   if (!SnippetExists(id))
                   {
                       return NotFound();
                   }
                   else
                   {
                       throw;
                   }
               }

               return NoContent();*/
        }

        // POST: api/Snippets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Snippet> PostSnippet(Snippet snippet)
        {
            snippet.Id = ++idTracker;

            snippet.Code = _encryptUtility.Encrypt(snippet.Code);

            snippets.Add(snippet);
            return CreatedAtAction(nameof(GetSnippet), new { id = snippet.Id }, snippet);
            /*string EncryptCode = Encrypt(snippet.Code);
            snippet.Code = EncryptCode;
            _context.Snippets.Add(snippet);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSnippet), new { id = snippet.Id }, snippet);*/
        }

        // DELETE: api/Snippets/5
        [HttpDelete("{id}")]
        public ActionResult DeleteSnippet(int id)
        {
            Snippet? findSnippet = snippets.Find(snippet => snippet.Id == id);

            if (findSnippet == null)
            {
                return NotFound("Snippet not found.");
            }
            snippets.Remove(findSnippet);
            return NoContent();

            /* if (_context.Snippets == null)
             {
                 return NotFound();
             }
             var snippet = await _context.Snippets.FindAsync(id);
             if (snippet == null)
             {
                 return NotFound();
             }

             _context.Snippets.Remove(snippet);
             await _context.SaveChangesAsync();

             return NoContent();*/
        }
    }
}
