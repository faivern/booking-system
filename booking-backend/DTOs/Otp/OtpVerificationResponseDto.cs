namespace booking_backend.DTOs.Otp;

/// <summary>
/// DTO for OTP verification response
/// </summary>
public record OtpVerificationResponseDto(
    bool Success,
    string Message,
    bool IsValid);
