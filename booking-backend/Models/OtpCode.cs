using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace booking_backend.Models
{
    /// <summary>
    /// Represents a one-time password code.
    /// </summary>
    [Table("tbl_OtpCode")]
    public class OtpCode
    {
        /// <summary>
        /// Gets or sets the unique identifier for the OTP code.
        /// </summary>
        [Key]
        [Column("OtpCodeId")]
        public int OtpCodeId { get; set; }
        
        /// <summary>
        /// Gets or sets the phone number associated with this OTP code.
        /// </summary>
        [Column("Phone")]
        public required string Phone { get; set; }
        
        /// <summary>
        /// Gets or sets the OTP code value.
        /// </summary>
        [Column("Code")]
        public required string Code { get; set; }
        
        /// <summary>
        /// Gets or sets the expiration time of the OTP code.
        /// </summary>
        [Column("ExpiresAt")]
        public DateTime ExpiresAt { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether the OTP code has been used.
        /// </summary>
        [Column("Used")]
        public bool Used { get; set; }
    }
}