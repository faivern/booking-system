namespace booking_backend.DTOs.Otp;

/// <summary>
/// DTO for requesting an OTP code
/// </summary>
public record RequestOtpDto(
    string PhoneNumber);
