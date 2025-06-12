 
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RWS_LBE_ACS.DTOs.Requests
{ 
    public class AuthRequest
    { 
        [JsonPropertyName("timestamp")]
        [Required]
        public string? Timestamp { get; set; }
         
        [JsonPropertyName("nonce")]
        [Required]
        public string? Nonce { get; set; }
         
        [JsonPropertyName("signature")]
        [Required]
        public string? Signature { get; set; }
    }
}

