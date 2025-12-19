namespace booking_backend.DTOs.Bookings;

/// <summary>
/// DTO for updating an existing booking
/// </summary>
public record UpdateBookingDto(
    DateTime StartTime,
    DateTime EndTime,
    string Status);
