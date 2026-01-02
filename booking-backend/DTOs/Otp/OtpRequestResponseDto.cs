namespace booking_backend.DTOs.Otp;

/// <summary>
/// DTO for OTP request response
/// </summary>
public record OtpRequestResponseDto(
    bool Success,
    string Message,
    string? PhoneNumber,
    int ExpiresInSeconds);
