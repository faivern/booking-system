namespace booking_backend.DTOs.Services;

/// <summary>
/// DTO for returning service data
/// </summary>
public record ServiceDto(
    int ServiceId,
    int BusinessId,
    string? BusinessName,
    string Name,
    string? Description,
    int DurationMinutes,
    decimal Price,
    bool IsActive);
