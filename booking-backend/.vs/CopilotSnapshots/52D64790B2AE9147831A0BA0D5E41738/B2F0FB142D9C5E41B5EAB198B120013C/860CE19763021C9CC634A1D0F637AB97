using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace booking_backend.Models
{
    /// <summary>
    /// Represents a customer in the system.
    /// </summary>
    [Table("tbl_Customer")]
    public class Customer
    {
        /// <summary>
        /// Gets or sets the unique identifier for the customer.
        /// </summary>
        [Key]
        [Column("CustomerId")]
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the first name of the customer.
        /// </summary>
        [Column("FirstName")]
        public required string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the customer.
        /// </summary>
        [Column("LastName")]
        public required string LastName { get; set; }
        
        /// <summary>
        /// Gets or sets the email address of the customer.
        /// </summary>
        [Column("Email")]
        public string? Email { get; set; }
        
        /// <summary>
        /// Gets or sets the phone number of the customer.
        /// </summary>
        [Column("Phone")]
        public string? Phone { get; set; }

        /// <summary>
        /// Gets or sets the collection of bookings for this customer.
        /// </summary>
        public ICollection<Booking> Bookings { get; set; } = [];
    }
}