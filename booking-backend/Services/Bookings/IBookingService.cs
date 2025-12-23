using booking_backend.DTOs.Bookings;

namespace booking_backend.Services.Bookings;

/// <summary>
/// Service interface for managing bookings
/// </summary>
public interface IBookingService
{
    /// <summary>
    /// Creates a new booking
    /// </summary>
    Task<BookingDto> CreateBookingAsync(CreateBookingDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a booking by ID
    /// </summary>
    Task<BookingDto?> GetBookingByIdAsync(int bookingId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all bookings for a customer
    /// </summary>
    Task<IEnumerable<BookingDto>> GetCustomerBookingsAsync(int customerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all bookings for a business
    /// </summary>
    Task<IEnumerable<BookingDto>> GetBusinessBookingsAsync(int businessId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing booking
    /// </summary>
    Task<BookingDto?> UpdateBookingAsync(int bookingId, UpdateBookingDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a booking
    /// </summary>
    Task<bool> DeleteBookingAsync(int bookingId, CancellationToken cancellationToken = default);
}
