using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RWS_LBE_ACS.Models
{
    [Table("audit_logs")]
    public class AuditLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("actor_id")]
        public string? ActorID { get; set; }

        [Column("method")]
        public string? Method { get; set; }

        [Column("path")]
        public string? Path { get; set; }

        [Column("status_code")]
        public long StatusCode { get; set; }

        [Column("client_ip")]
        public string? ClientIP { get; set; }

        [Column("user_agent")]
        public string? UserAgent { get; set; }

        [Column("request_body", TypeName = "nvarchar(max)")]
        public string? RequestBody { get; set; }

        [Column("response_body", TypeName = "nvarchar(max)")]
        public string? ResponseBody { get; set; }

        [Column("latency_ms")]
        public long LatencyMs { get; set; }
    }
}
