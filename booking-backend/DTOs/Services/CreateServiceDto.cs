namespace booking_backend.DTOs.Services;

/// <summary>
/// DTO for creating a new service
/// </summary>
public record CreateServiceDto(
    int BusinessId,
    string Name,
    string? Description,
    int DurationMinutes,
    decimal Price);
