using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Snipper_Snippet_API.Models;
using Snipper_Snippet_API.Service;

namespace Snipper_Snippet_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        //private readonly SnippetContext _context;

        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }


        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            User? user = HttpContext.Items["User"] as User;
            if (user == null)
            {
                return Unauthorized();
            }
            return Ok(new {Id = user.Id, Email = user.Email});
         /* if (_context.User == null)
          {
              return NotFound();
          }
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;*/
        }

   

            // POST: api/Users
            // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
            [HttpPost]
            public ActionResult<User> CreateUser()
        {
                User? user = HttpContext.Items["User"] as User;
                if (user == null)
                {
                    return BadRequest();
                }
                return Ok(new {Id = user.Id, Email = user.Email});
         /* if (_context.User == null)
          {
              return Problem("Entity set 'SnippetContext.User'  is null.");
          }
            byte[] salt = Salting();
            user.Password = Hashing(user.Password, salt);
            _context.User.Add(user);
            await _context.SaveChangesAsync();
            user.Password = null;

            return CreatedAtAction("GetUser", new { id = user.Id }, user);*/
        }
    }
}
