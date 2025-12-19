namespace booking_backend.DTOs.Services;

/// <summary>
/// DTO for updating an existing service
/// </summary>
public record UpdateServiceDto(
    string Name,
    string? Description,
    int DurationMinutes,
    decimal Price,
    bool IsActive);
