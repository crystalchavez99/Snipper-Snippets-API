using Snipper_Snippet_API.Models;
using Snipper_Snippet_API.Service;
using System.Text;

namespace Snipper_Snippet_API.Middleware
{
    public class basicAuth
    {
        private readonly RequestDelegate _next;
        private readonly UserService _userService;
        public basicAuth(RequestDelegate next, UserService userService)
        {
            _next = next;
            _userService = userService; 
        }
        public async Task InvokeAsync(HttpContext context)
        {
            string? authHeader = context.Request.Headers.Authorization;
            if (authHeader != null && authHeader.StartsWith("Basic")) {
                // parse from header
                var encodeEmailPassword = authHeader.Substring("Basic".Length).Trim();
                var decodeEmailPassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodeEmailPassword));
                var seperate = decodeEmailPassword.IndexOf(":");
                var email = decodeEmailPassword.Substring(0, seperate);
                var password = decodeEmailPassword.Substring(seperate + 1);

                User? user;

                if(context.Request.Method == "POST")
                {
                    user = _userService.CreateUser(email, password);
                }
                else
                {
                    user = _userService.AuthenticateUser(email, password);
                };

                if(user == null)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Unauth!");
                    return;
                }

                context.Items.Add("User", user);
            }
            await _next(context);
        }
    }
}
