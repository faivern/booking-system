using Microsoft.EntityFrameworkCore;
using booking_backend.Models;

namespace booking_backend.Data
{
    /// <summary>
    /// Database context for the booking system.
    /// </summary>
    public class BookingSystemDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BookingSystemDbContext"/> class.
        /// </summary>
        /// <param name="options">The database context options</param>
        public BookingSystemDbContext(DbContextOptions<BookingSystemDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the bookings in the database.
        /// </summary>
        public DbSet<Booking> Bookings { get; set; }

        /// <summary>
        /// Gets or sets the businesses in the database.
        /// </summary>
        public DbSet<Business> Businesses { get; set; }

        /// <summary>
        /// Gets or sets the customers in the database.
        /// </summary>
        public DbSet<Customer> Customers { get; set; }

        /// <summary>
        /// Gets or sets the opening hours in the database.
        /// </summary>
        public DbSet<OpeningHour> OpeningHours { get; set; }

        /// <summary>
        /// Gets or sets the OTP codes in the database.
        /// </summary>
        public DbSet<OtpCode> OtpCodes { get; set; }

        /// <summary>
        /// Gets or sets the services in the database.
        /// </summary>
        public DbSet<Service> Services { get; set; }
    }
}