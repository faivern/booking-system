namespace booking_backend.DTOs.Businesses;

/// <summary>
/// DTO for updating an existing business
/// </summary>
public record UpdateBusinessDto(
    string Name,
    string? Description,
    string? Email,
    string? Phone);
