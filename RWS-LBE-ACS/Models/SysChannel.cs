
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RWS_LBE_ACS.Models
{
    [Table("sys_channel")]
    public class SysChannel
    {
        [Key]
        [Column("id", TypeName = "bigint")]           
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonPropertyName("id")]
        public long Id { get; set; }                   

        [Column("app_id")]
        [Required]
        [JsonPropertyName("app_id")]
        public string? AppId { get; set; }

        [Column("app_key")]
        [MaxLength(100)]
        [Required]
        [JsonPropertyName("app_key")]
        public string? AppKey { get; set; }

        [Column("status")]
        [Required]
        [JsonPropertyName("status")]
        public string? Status { get; set; } 

        [Column("sig_method")]
        [MaxLength(255)]
        [Required]
        [JsonPropertyName("sig_method")]
        public string? SigMethod { get; set; }

        [Column("create_time")]
        [JsonPropertyName("create_time")]
        public DateTime CreateTime { get; set; }

        [Column("update_time")]
        [JsonPropertyName("update_time")]
        public DateTime UpdateTime { get; set; }
    }
}
