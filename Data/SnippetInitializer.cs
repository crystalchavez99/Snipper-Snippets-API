using Newtonsoft.Json;
using Snipper_Snippet_API.Controllers;
using Snipper_Snippet_API.Models;

namespace Snipper_Snippet_API.Data
{
    public static class SnippetInitializer
    {
        public static void Initialize(IWebHostEnvironment env)
        {
            var snippetsJson = File.ReadAllText(Path.Combine(env.ContentRootPath, "Data", "seedData.json"));
            var fromJson = JsonConvert.DeserializeObject<List<Snippet>>(snippetsJson);

            SnippetsController.snippets.AddRange(fromJson?.ToList() ?? new List<Snippet>());
            SnippetsController.idTracker = SnippetsController.snippets.Count();

        }
    }
}
