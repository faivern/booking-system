using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace booking_backend.Models
{
    /// <summary>
    /// Represents a business in the system.
    /// </summary>
    [Table("tbl_Business")]
    public class Business
    {
        /// <summary>
        /// Gets or sets the unique identifier for the business.
        /// </summary>
        [Key]
        [Column("BusinessId")]
        public int BusinessId { get; set; }

        /// <summary>
        /// Gets or sets the name of the business.
        /// </summary>
        [Column("Name")]
        public required string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the business.
        /// </summary>
        [Column("Description")]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the email address of the business.
        /// </summary>
        [Column("Email")]
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the business.
        /// </summary>
        [Column("Phone")]
        public string? Phone { get; set; }

        /// <summary>
        /// Gets or sets the collection of bookings for this business.
        /// </summary>
        public ICollection<Booking> Bookings { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of services offered by this business.
        /// </summary>
        public ICollection<Service> Services { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of opening hours for this business.
        /// </summary>
        public ICollection<OpeningHour> OpeningHours { get; set; } = [];
    }
}