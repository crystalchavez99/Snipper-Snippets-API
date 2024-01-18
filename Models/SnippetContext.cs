using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Snipper_Snippet_API.Models
{
    public class SnippetContext : DbContext
    {
        public SnippetContext(DbContextOptions<SnippetContext> options): base(options) { }
        public DbSet<Snippet> Snippets { get; set; } = null!;
    }
}
