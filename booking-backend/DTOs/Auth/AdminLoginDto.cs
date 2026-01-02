namespace booking_backend.DTOs.Auth;

/// <summary>
/// DTO for admin login request
/// </summary>
public record AdminLoginDto(
    string Username,
    string Password);
