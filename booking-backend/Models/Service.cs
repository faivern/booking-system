using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace booking_backend.Models
{
    /// <summary>
    /// Represents a service offered by a business.
    /// </summary>
    [Table("tbl_Service")]
    public class Service
    {
        /// <summary>
        /// Gets or sets the unique identifier for the service.
        /// </summary>
        [Key]
        [Column("ServiceId")]
        public int ServiceId { get; set; }
        
        /// <summary>
        /// Gets or sets the business identifier.
        /// </summary>
        [Column("BusinessId")]
        public int BusinessId { get; set; }
        
        /// <summary>
        /// Gets or sets the name of the service.
        /// </summary>
        [Column("Name")]
        public required string Name { get; set; }
        
        /// <summary>
        /// Gets or sets the description of the service.
        /// </summary>
        [Column("Description")]
        public string? Description { get; set; }
        
        /// <summary>
        /// Gets or sets the duration of the service in minutes.
        /// </summary>
        [Column("DurationMinutes")]
        public int DurationMinutes { get; set; }
        
        /// <summary>
        /// Gets or sets the price of the service.
        /// </summary>
        [Column("Price")]
        public decimal Price { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether the service is active.
        /// </summary>
        [Column("IsActive")]
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the associated business.
        /// </summary>
        [ForeignKey("BusinessId")]
        public Business? Business { get; set; }

        /// <summary>
        /// Gets or sets the collection of bookings for this service.
        /// </summary>
        public ICollection<Booking> Bookings { get; set; } = [];
    }
}