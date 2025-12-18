using Microsoft.EntityFrameworkCore;
using booking_backend.Models;

namespace booking_backend.Data
{
    public class BookingSystemDbContext : DbContext
    {
        public BookingSystemDbContext(DbContextOptions<BookingSystemDbContext> options)
            : base(options)
        {
        }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<OpeningHour> OpeningHours { get; set; }
        public DbSet<OtpCode> OtpCodes { get; set; }
        public DbSet<Service> Services { get; set; }

        }
}
