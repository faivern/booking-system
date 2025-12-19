namespace booking_backend.DTOs.OpeningHours;

/// <summary>
/// DTO for updating an existing opening hour
/// </summary>
public record UpdateOpeningHourDto(
    byte DayOfWeek,
    TimeOnly StartTime,
    TimeOnly EndTime);
