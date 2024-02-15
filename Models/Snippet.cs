using Newtonsoft.Json;

namespace Snipper_Snippet_API.Models
{
    public class Snippet
    {
       /* public Snippet(int Id, string Language, string Code)
        {
            this.Id = Id;
            this.Language = Language;
            this.Code = Code;
        }*/

        public int Id { get; set; }
        public string Language { get; set; }
        public string Code { get; set; }
    }
}
