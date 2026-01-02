# OTP Service Documentation

## Overview

The OTP (One-Time Password) service provides secure SMS-based authentication for your booking system. It implements industry best practices including:

- **6-digit random code** - Secure, easy to remember
- **2-minute expiry** - Balances security with usability
- **Rate limiting** - Prevents brute force attacks (3 attempts per 15 minutes)
- **Used flag tracking** - Ensures one-time use
- **Automatic cleanup** - Removes expired codes

## Features

### 1. OTP Generation
- Cryptographically secure random number generation
- 6-digit format (000000-999999)
- Automatic cleanup of previous unused OTPs for same phone
- 2-minute expiration time

### 2. SMS Integration
- Support for multiple SMS providers (Twilio, AWS SNS, Azure Communication Services, Vonage)
- Development mode logging (logs OTP to console/logs instead of sending SMS)
- Phone number validation

### 3. Rate Limiting
- Maximum 3 OTP requests per phone number every 15 minutes
- Prevents brute force and spam attacks
- Returns 429 (Too Many Requests) when exceeded

### 4. Verification
- Validates OTP before expiry
- One-time use enforcement (marks OTP as used)
- Case-insensitive code matching with trim

### 5. Monitoring
- Attempt tracking
- Remaining attempts retrieval
- Expired OTP cleanup

## API Endpoints

### 1. Request OTP
```
POST /api/otp/request
Content-Type: application/json

{
    "phoneNumber": "+1234567890"
}
```

**Response (200 OK):**
```json
{
    "success": true,
    "message": "OTP sent successfully to your phone",
    "phoneNumber": "+1234567890",
    "expiresInSeconds": 120
}
```

**Response (400 Bad Request):**
```json
{
    "success": false,
    "message": "Invalid phone number format",
    "phoneNumber": null,
    "expiresInSeconds": 0
}
```

**Response (429 Too Many Requests):**
```json
{
    "success": false,
    "message": "Too many OTP requests. Please try again later.",
    "phoneNumber": null,
    "expiresInSeconds": 0
}
```

### 2. Verify OTP
```
POST /api/otp/verify
Content-Type: application/json

{
    "phoneNumber": "+1234567890",
    "code": "123456"
}
```

**Response (200 OK):**
```json
{
    "success": true,
    "message": "OTP verified successfully",
    "isValid": true
}
```

**Response (401 Unauthorized):**
```json
{
    "success": false,
    "message": "Invalid OTP code",
    "isValid": false
}
```

### 3. Get Remaining Attempts
```
GET /api/otp/remaining-attempts/{phoneNumber}
```

**Response (200 OK):**
```json
{
    "phoneNumber": "+1234567890",
    "remainingAttempts": 2
}
```

## Configuration

### Environment Settings

The SMS service automatically detects the environment:

**Development Mode:**
- OTP codes are logged to the application logs instead of sent via SMS
- Check logs for the generated OTP code

**Production Mode:**
- Requires SMS provider configuration
- Sends actual SMS messages

### SMS Provider Integration

To integrate with an actual SMS provider, modify `Services/Sms/SmsService.cs`:

#### Example: Twilio Integration
```csharp
using Twilio;
using Twilio.Rest.Api.V2010.Account;

public async Task<bool> SendSmsAsync(string phoneNumber, string message, CancellationToken cancellationToken = default)
{
    try
    {
        var accountSid = _configuration["Twilio:AccountSid"];
        var authToken = _configuration["Twilio:AuthToken"];
        var fromNumber = _configuration["Twilio:PhoneNumber"];

        TwilioClient.Init(accountSid, authToken);

        var sms = await MessageResource.CreateAsync(
            body: message,
            from: new Twilio.Types.PhoneNumber(fromNumber),
            to: new Twilio.Types.PhoneNumber(phoneNumber)
        );

        _logger.LogInformation("SMS sent successfully. SID: {SmsSid}", sms.Sid);
        return true;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error sending SMS via Twilio");
        return false;
    }
}
```

#### Example: AWS SNS Integration
```csharp
using Amazon.SNS;
using Amazon.SNS.Model;

public async Task<bool> SendSmsAsync(string phoneNumber, string message, CancellationToken cancellationToken = default)
{
    try
    {
        var snsClient = new AmazonSNSClient();
        
        var request = new PublishRequest
        {
            Message = message,
            PhoneNumber = phoneNumber,
            MessageAttributes = new Dictionary<string, MessageAttributeValue>
            {
                { "AWS.SNS.SMS.SenderID", new MessageAttributeValue { StringValue = "BookingSystem", DataType = "String" } },
                { "AWS.SNS.SMS.SMSType", new MessageAttributeValue { StringValue = "Transactional", DataType = "String" } }
            }
        };

        var response = await snsClient.PublishAsync(request, cancellationToken);
        _logger.LogInformation("SMS sent successfully via AWS SNS. MessageId: {MessageId}", response.MessageId);
        return true;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error sending SMS via AWS SNS");
        return false;
    }
}
```

## Security Best Practices Implemented

1. **Cryptographic Security** - Uses `RNGCryptoServiceProvider` for random generation
2. **Rate Limiting** - Prevents brute force attacks
3. **Expiration** - 2-minute time window reduces window of vulnerability
4. **Single Use** - OTP can only be used once
5. **Input Validation** - Phone number format validation
6. **Logging** - Security events are logged for audit
7. **Code Separation** - SMS service decoupled from OTP logic

## Database Cleanup

The `CleanupExpiredOtpsAsync` method should be called periodically to remove expired codes:

### Option 1: Hosted Service (Recommended)
```csharp
// Add to Program.cs
builder.Services.AddHostedService<OtpCleanupHostedService>();
```

### Option 2: Background Job
Use Hangfire or similar job scheduler to call cleanup periodically:
```csharp
RecurringJob.AddOrUpdate<IOtpService>(
    "otp-cleanup",
    x => x.CleanupExpiredOtpsAsync(CancellationToken.None),
    Cron.MinuteInterval(5)); // Every 5 minutes
```

### Option 3: Manual Cleanup
```csharp
// In your admin controller or background task
await _otpService.CleanupExpiredOtpsAsync(cancellationToken);
```

## Development Testing

In development mode, OTP codes are logged. Check your application logs:

```
?? SMS Message (DEV MODE) to +1234567890: Your OTP code is: 123456. Valid for 2 minutes. Do not share this code.
```

Use this code to test the `/api/otp/verify` endpoint.

## Future Enhancements

1. **Distributed Cache** - Replace in-memory attempt tracking with Redis for scalability
2. **SMS Analytics** - Track delivery rates and success metrics
3. **Backup Codes** - Provide alternative verification methods
4. **Email OTP** - Support email-based OTP as fallback
5. **Adjustable Settings** - Make expiry, length, and attempts configurable
6. **OTP History** - Track OTP usage for audit trails

## Troubleshooting

| Issue | Solution |
|-------|----------|
| OTP not received | Check if SMS provider is configured in production mode |
| "Too many requests" error | User must wait 15 minutes before requesting new OTP |
| OTP expired | User must request new OTP (2-minute expiry) |
| "Invalid OTP code" | Check if code matches exactly (case-sensitive for digits) |

## Constants Configuration

To modify OTP behavior, adjust these constants in `OtpService.cs`:

```csharp
private const int OtpExpiryMinutes = 2;      // OTP validity duration
private const int OtpLength = 6;             // Number of digits
private const int MaxAttempts = 3;           // Max requests per window
private const int AttemptsResetMinutes = 15; // Reset window duration
```
