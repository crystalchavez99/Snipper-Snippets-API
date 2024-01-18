using Newtonsoft.Json;

namespace Snipper_Snippet_API.Models
{
    public class Snippet
    {
        public Snippet(int Id, string Langauge, string Code)
        {
            this.Id = Id;
            this.Language = Langauge;
            this.Code = Code;
        }

        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("language")]
        public string Language { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
    }
}
