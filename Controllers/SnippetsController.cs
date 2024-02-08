using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Snipper_Snippet_API.Models;

namespace Snipper_Snippet_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SnippetsController : ControllerBase
    {
        //private readonly SnippetContext _context; used for db but need to learn how to do in memory

        public static List<Snippet> snippets = new List<Snippet>();

        public static int idTracker = snippets.Count();

        private readonly ILogger<SnippetsController> _logger;

            public SnippetsController(ILogger<SnippetsController> logger)
        {
            _logger = logger;
        }

        // GET: api/Snippets
        [HttpGet]
        public ActionResult<List<Snippet>> GetSnippets()
        {
            List<Snippet> allSnippets = snippets.ToList();
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

            return findSnippet;
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

            findSnippet.Code = snippet.Code;
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

        /*private bool SnippetExists(int id)
        {
            return (_context.Snippets?.Any(e => e.Id == id)).GetValueOrDefault();
        }*/
        private string Encrypt(string plainText)
        {
            byte[] key = Encoding.UTF8.GetBytes("temporary_secret_key");
            byte[] iv = Encoding.UTF8.GetBytes("temporary_init_vector");
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        public string Decrypt(string encryptedText)
        {
            byte[] key = Encoding.UTF8.GetBytes("temporary_secret_key");
            byte[] iv = Encoding.UTF8.GetBytes("temporary_init_vector");
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(encryptedText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader swEncrypt = new StreamReader(csDecrypt))
                        {
                            return swEncrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
