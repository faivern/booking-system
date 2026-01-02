using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace booking_backend.Models
{
    [Table("tbl_OtpCode")]
    public class OtpCode
    {
        [Key]
        [Column("OtpCodeId")]
        public int OtpCodeId { get; set; }
        
        [Column("Phone")]
        public required string Phone { get; set; }
        
        [Column("Code")]
        public required string Code { get; set; }
        
        [Column("ExpiresAt")]
        public DateTime ExpiresAt { get; set; }
        
        [Column("Used")]
        public bool Used { get; set; }
    }
}