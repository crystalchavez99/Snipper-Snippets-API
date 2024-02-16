using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        private readonly JwtSettings _jwtSettings;

        public UsersController(UserService userService, JwtSettings jwtSettings)
        {
            _userService = userService;
            _jwtSettings = jwtSettings;
        }


        // GET: api/Users/5
        [HttpGet]
        [Authorize]
        public  ActionResult<User> GetUser()
        {
            var user = User;
            if (user == null)
            {
                return Unauthorized(new { error = "Coudln't access data." });
            }
            int Id = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0"); 
            string Email = user.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;

            return Ok(new {Id=Id, Email=Email});
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

        [HttpPost("login")]
        public ActionResult Login()
        {
            User? user = HttpContext.Items["User"] as User;
            if (user == null)
            {
                return Unauthorized(new { error = "Invalid email or password" });
            }
            var token = _userService.GenerateToken(user);
            return Ok(new {token, user.Id, user.Email});
        }
    }
}
