namespace booking_backend.DTOs.Otp;

/// <summary>
/// DTO for verifying an OTP code
/// </summary>
public record VerifyOtpDto(
    string PhoneNumber,
    string Code);
