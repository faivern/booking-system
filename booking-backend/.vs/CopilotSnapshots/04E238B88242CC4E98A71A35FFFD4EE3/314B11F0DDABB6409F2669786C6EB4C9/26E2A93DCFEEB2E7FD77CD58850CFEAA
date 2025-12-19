using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace booking_backend.Models
{
    [Table("tbl_Customer")]
    public class Customer
    {
        [Key]
        [Column("CustomerId")]
        public int CustomerId { get; set; }

        [Column("FirstName")]
        public required string FirstName { get; set; }

        [Column("LastName")]
        public required string LastName { get; set; }
        
        [Column("Email")]
        public string? Email { get; set; }
        
        [Column("Phone")]
        public string? Phone { get; set; }
    }
}