﻿namespace Snipper_Snippet_API.Models
{
    public class JwtSettings
    {
        public string? Secret { get; set; }
        public string? Audience { get; set; }
        public string? Issuer { get; set; }
    }
}
