using Microsoft.EntityFrameworkCore;
namespace Snipper_Snippet_API.Models;

public class SnipperContext : DbContext
{
    public SnipperContext(DbContextOptions<SnipperContext> options) : base(options) { }
    public DbSet<Snippet> Snippets { get; set; }
}
