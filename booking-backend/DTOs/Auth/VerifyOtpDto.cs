namespace booking_backend.DTOs.Auth;

/// <summary>
/// DTO for verifying an OTP code
/// </summary>
public record VerifyOtpDto(string Phone, string Code);
