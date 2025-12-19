namespace booking_backend.DTOs.Businesses;

/// <summary>
/// DTO for returning business data
/// </summary>
public record BusinessDto(
    int BusinessId,
    string Name,
    string? Description,
    string? Email,
    string? Phone);
