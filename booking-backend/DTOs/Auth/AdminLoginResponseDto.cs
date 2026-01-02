namespace booking_backend.DTOs.Auth;

/// <summary>
/// DTO for admin login response
/// </summary>
public record AdminLoginResponseDto(
    bool Success,
    string? Message,
    string? SessionId);
