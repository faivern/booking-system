namespace booking_backend.Services.Sms;

/// <summary>
/// SMS service implementation for development/testing
/// In production, replace with actual SMS provider (Twilio, AWS SNS, etc.)
/// </summary>
public class SmsService : ISmsService
{
    private readonly ILogger<SmsService> _logger;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="SmsService"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="configuration">The application configuration.</param>
    public SmsService(ILogger<SmsService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// Sends an SMS message to the specified phone number
    /// </summary>
    public async Task<bool> SendSmsAsync(string phoneNumber, string message, CancellationToken cancellationToken = default)
    {
        try
        {
            // Validate phone number
            if (string.IsNullOrWhiteSpace(phoneNumber) || !IsValidPhoneNumber(phoneNumber))
            {
                _logger.LogWarning("Invalid phone number: {PhoneNumber}", phoneNumber);
                return false;
            }

            // In development, log the message instead of sending
            if (IsDevEnvironment())
            {
                _logger.LogWarning("?? SMS Message (DEV MODE) to {PhoneNumber}: {Message}", phoneNumber, message);
                return await Task.FromResult(true);
            }

            // TODO: Implement actual SMS provider integration
            // Example providers:
            // - Twilio: https://www.twilio.com/
            // - AWS SNS: https://aws.amazon.com/sns/
            // - Vonage (Nexmo): https://www.vonage.com/
            // - Azure Communication Services: https://azure.microsoft.com/en-us/services/communication-services/
            
            _logger.LogError("SMS service not configured. Message not sent to {PhoneNumber}", phoneNumber);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending SMS to {PhoneNumber}", phoneNumber);
            return false;
        }
    }

    /// <summary>
    /// Validates phone number format
    /// </summary>
    private static bool IsValidPhoneNumber(string phoneNumber)
    {
        // Basic validation: phone should contain only digits and optional + prefix
        // Adjust pattern based on your requirements
        return System.Text.RegularExpressions.Regex.IsMatch(
            phoneNumber,
            @"^\+?[\d\s\-()]{7,}$");
    }

    /// <summary>
    /// Checks if running in development environment
    /// </summary>
    private bool IsDevEnvironment()
    {
        var environment = _configuration["ASPNETCORE_ENVIRONMENT"] ?? "Production";
        return environment.Equals("Development", StringComparison.OrdinalIgnoreCase);
    }
}
