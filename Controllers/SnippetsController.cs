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
        private readonly SnippetContext _context;

            public SnippetsController(SnippetContext context)
        {
            _context = context;
        }

        // GET: api/Snippets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Snippet>>> GetSnippets()
        {
          if (_context.Snippets == null)
          {
              return NotFound();
          }
            return await _context.Snippets.ToListAsync();
        }

        // GET: api/Snippets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Snippet>> GetSnippet(int id)
        {
          if (_context.Snippets == null)
          {
              return NotFound();
          }
            var snippet = await _context.Snippets.FindAsync(id);

            if (snippet == null)
            {
                return NotFound();
            }

            return snippet;
        }

        // PUT: api/Snippets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSnippet(int id, Snippet snippet)
        {
            if (id != snippet.Id)
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

            return NoContent();
        }

        // POST: api/Snippets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Snippet>> PostSnippet(Snippet snippet)
        {
            string EncryptCode = Encrypt(snippet.Code);
            snippet.Code = EncryptCode;
            _context.Snippets.Add(snippet);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSnippet), new { id = snippet.Id }, snippet);
        }

        // DELETE: api/Snippets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSnippet(int id)
        {
            if (_context.Snippets == null)
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

            return NoContent();
        }

        private bool SnippetExists(int id)
        {
            return (_context.Snippets?.Any(e => e.Id == id)).GetValueOrDefault();
        }
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
    }
}
