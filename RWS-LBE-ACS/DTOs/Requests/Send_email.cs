using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RWS_LBE_ACS.DTOs.Requests
{
    public class Send_email
    {
        public class SendPlainTextEmailRequest
        {
            [Required]
            [JsonPropertyName("email")]
            public string Email { get; set; } = string.Empty;

            [Required]
            [JsonPropertyName("subject")]
            public string Subject { get; set; } = string.Empty;

            [Required]
            [JsonPropertyName("plain_text")]
            public string PlainText { get; set; } = string.Empty;

            [JsonPropertyName("attachments")]
            public List<Dictionary<string, string>>? Attachments { get; set; }
        }

        public class SendHtmlEmailRequest
        {
            [Required]
            [JsonPropertyName("email")]
            public string Email { get; set; } = string.Empty;

            [Required]
            [JsonPropertyName("subject")]
            public string Subject { get; set; } = string.Empty;

            [Required]
            [JsonPropertyName("html")]
            public string Html { get; set; } = string.Empty;

            [JsonPropertyName("attachments")]
            public List<Dictionary<string, string>>? Attachments { get; set; }
        }

        public class SendTemplateEmailRequest
        {
            [Required]
            [JsonPropertyName("email")]
            public string Email { get; set; } = string.Empty;

            [Required]
            [JsonPropertyName("subject")]
            public string Subject { get; set; } = string.Empty;

            [Required]
            [JsonPropertyName("data")]
            public Dictionary<string, object>? Data { get; set; }

            [JsonPropertyName("attachments")]
            public List<Dictionary<string, string>>? Attachments { get; set; }
        }
    }
}
