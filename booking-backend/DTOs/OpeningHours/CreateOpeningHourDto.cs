namespace booking_backend.DTOs.OpeningHours;

/// <summary>
/// DTO for creating a new opening hour
/// </summary>
public record CreateOpeningHourDto(
    int BusinessId,
    byte DayOfWeek,
    TimeOnly StartTime,
    TimeOnly EndTime);
