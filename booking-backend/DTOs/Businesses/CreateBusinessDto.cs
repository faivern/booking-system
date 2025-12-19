namespace booking_backend.DTOs.Businesses;

/// <summary>
/// DTO for creating a new business
/// </summary>
public record CreateBusinessDto(
    string Name,
    string? Description,
    string? Email,
    string? Phone);
