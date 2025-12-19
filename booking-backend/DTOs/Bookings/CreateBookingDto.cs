namespace booking_backend.DTOs.Bookings;

/// <summary>
/// DTO for creating a new booking
/// </summary>
public record CreateBookingDto(
    int BusinessId,
    int ServiceId,
    int CustomerId,
    DateTime StartTime,
    DateTime EndTime);
