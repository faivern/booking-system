namespace booking_backend.DTOs.Bookings;

/// <summary>
/// DTO for returning booking data
/// </summary>
public record BookingDto(
    int BookingId,
    int BusinessId,
    string? BusinessName,
    int ServiceId,
    string? ServiceName,
    int CustomerId,
    string? CustomerName,
    DateTime StartTime,
    DateTime EndTime,
    string Status,
    DateTime CreatedAt);
