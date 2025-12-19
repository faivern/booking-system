namespace booking_backend.DTOs.OpeningHours;

/// <summary>
/// DTO for returning opening hour data
/// </summary>
public record OpeningHourDto(
    int OpeningHourId,
    int BusinessId,
    string? BusinessName,
    byte DayOfWeek,
    TimeOnly StartTime,
    TimeOnly EndTime);
