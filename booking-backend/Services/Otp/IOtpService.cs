using booking_backend.DTOs.Otp;

namespace booking_backend.Services.Otp;

/// <summary>
/// Service interface for OTP operations
/// </summary>
public interface IOtpService
{
    /// <summary>
    /// Generates and sends an OTP code to the specified phone number
    /// </summary>
    /// <param name="phoneNumber">The phone number to send OTP to</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>OTP request response</returns>
    Task<OtpRequestResponseDto> GenerateAndSendOtpAsync(string phoneNumber, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifies an OTP code for the specified phone number
    /// </summary>
    /// <param name="phoneNumber">The phone number</param>
    /// <param name="code">The OTP code to verify</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Verification response</returns>
    Task<OtpVerificationResponseDto> VerifyOtpAsync(string phoneNumber, string code, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the number of OTP attempts remaining for a phone number
    /// </summary>
    /// <param name="phoneNumber">The phone number</param>
    /// <returns>Number of remaining attempts</returns>
    int GetRemainingAttempts(string phoneNumber);

    /// <summary>
    /// Cleans up expired OTP codes
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    Task CleanupExpiredOtpsAsync(CancellationToken cancellationToken = default);
}
