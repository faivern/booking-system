using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace booking_backend.Models
{
    /// <summary>
    /// Represents a booking in the system.
    /// </summary>
    [Table("tbl_Booking")]
    public class Booking
    {
        /// <summary>
        /// Gets or sets the unique identifier for the booking.
        /// </summary>
        [Key]
        [Column("BookingId")]
        public int BookingId { get; set; }

        /// <summary>
        /// Gets or sets the business identifier.
        /// </summary>
        [Column("BusinessId")]
        public int BusinessId { get; set; }

        /// <summary>
        /// Gets or sets the service identifier.
        /// </summary>
        [Column("ServiceId")]
        public int ServiceId { get; set; }

        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        [Column("CustomerId")]
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the start time of the booking.
        /// </summary>
        [Column("StartTime")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time of the booking.
        /// </summary>
        [Column("EndTime")]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Gets or sets the status of the booking.
        /// </summary>
        [Column("Status")]
        public required string Status { get; set; }

        /// <summary>
        /// Gets or sets the creation timestamp of the booking.
        /// </summary>
        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the associated business.
        /// </summary>
        [ForeignKey("BusinessId")]
        public Business? Business { get; set; }

        /// <summary>
        /// Gets or sets the associated service.
        /// </summary>
        [ForeignKey("ServiceId")]
        public Service? Service { get; set; }

        /// <summary>
        /// Gets or sets the associated customer.
        /// </summary>
        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }
    }
}