using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Snipper_Snippet_API.Models;

namespace Snipper_Snippet_API.Models
{
    public class SnippetContext : DbContext
    {
        public SnippetContext(DbContextOptions<SnippetContext> options): base(options) { }
        public DbSet<Snippet> Snippets { get; set; } = null!;
        public DbSet<Snipper_Snippet_API.Models.User> User { get; set; } = default!;
    }
}
