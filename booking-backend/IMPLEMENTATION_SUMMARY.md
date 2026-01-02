# In-House OTP Service - Implementation Summary

## What Was Created

### ? DTOs (Data Transfer Objects)
1. **RequestOtpDto** - Request containing phone number
2. **VerifyOtpDto** - Verification request with phone and code
3. **OtpRequestResponseDto** - Response after OTP request
4. **OtpVerificationResponseDto** - Response after verification

### ? Services

#### SMS Service (`Services/Sms/`)
- **ISmsService** - Interface for SMS operations
- **SmsService** - Implementation with:
  - Development mode logging (logs to console instead of sending)
  - Production mode support (ready for Twilio, AWS SNS, Azure, Vonage)
  - Phone number validation
  - Error handling and logging

#### OTP Service (`Services/Otp/`)
- **IOtpService** - Interface for OTP operations
- **OtpService** - Implementation with:
  - 6-digit cryptographically secure random code generation
  - 2-minute expiration time
  - Rate limiting (3 attempts per 15 minutes)
  - One-time use enforcement
  - Automatic expiration cleanup
  - In-memory attempt tracking

### ? Controllers
- **OtpController** - 3 endpoints:
  1. `POST /api/otp/request` - Generate and send OTP
  2. `POST /api/otp/verify` - Verify OTP code
  3. `GET /api/otp/remaining-attempts/{phoneNumber}` - Check remaining attempts

### ? Configuration
- Updated `Program.cs` with service registrations
- All services properly injected and configured

## Key Features

| Feature | Details |
|---------|---------|
| **Code Format** | 6 random digits (000000-999999) |
| **Expiration** | 2 minutes from generation |
| **Attempts** | Max 3 requests per 15-minute window |
| **Security** | Cryptographic RNG, one-time use, rate limiting |
| **SMS** | Development: logs to console, Production: ready for SMS providers |
| **Database** | Uses existing `OtpCode` model with `Used` boolean tracking |
| **Validation** | Phone number format validation |
| **Cleanup** | Expired codes can be deleted via `CleanupExpiredOtpsAsync()` |

## API Endpoints

### 1. Request OTP
```
POST /api/otp/request
{
    "phoneNumber": "+1234567890"
}

Response:
{
    "success": true,
    "message": "OTP sent successfully to your phone",
    "phoneNumber": "+1234567890",
    "expiresInSeconds": 120
}
```

### 2. Verify OTP
```
POST /api/otp/verify
{
    "phoneNumber": "+1234567890",
    "code": "123456"
}

Response:
{
    "success": true,
    "message": "OTP verified successfully",
    "isValid": true
}
```

### 3. Get Remaining Attempts
```
GET /api/otp/remaining-attempts/+1234567890

Response:
{
    "phoneNumber": "+1234567890",
    "remainingAttempts": 2
}
```

## Development vs Production

### Development Mode (Default)
- OTP codes are logged to application logs
- No actual SMS is sent
- Perfect for testing and development
- Look for logs: `?? SMS Message (DEV MODE) to {phoneNumber}: Your OTP code is: {code}`

### Production Mode
- Requires SMS provider configuration (Twilio, AWS SNS, Azure, Vonage)
- Sends actual SMS messages
- Update `SmsService.cs` with your provider's SDK

## Database Model
Uses existing `OtpCode` table with:
- `OtpCodeId` - Primary key
- `Phone` - Phone number (string)
- `Code` - 6-digit code (string)
- `ExpiresAt` - Expiration timestamp (DateTime)
- `Used` - Boolean flag (bool)

## Error Handling

| Status Code | Scenario |
|-------------|----------|
| 200 OK | Success |
| 400 Bad Request | Invalid phone number or missing fields |
| 401 Unauthorized | Invalid/expired OTP code |
| 429 Too Many Requests | Rate limit exceeded (3 attempts per 15 min) |
| 500 Internal Server Error | Server error |

## Security Implementation

? **Cryptographic Security** - Uses RNGCryptoServiceProvider for randomness
? **Rate Limiting** - Prevents brute force (3 attempts per 15 minutes)
? **Expiration** - 2-minute window limits attack surface
? **One-Time Use** - `Used` flag prevents replay attacks
? **Input Validation** - Phone number format validation
? **Audit Logging** - All security events logged

## Next Steps

1. **SMS Provider Integration** (when going to production)
   - Choose provider: Twilio, AWS SNS, Azure Communication Services, or Vonage
   - Update `SmsService.cs` with provider SDK
   - Configure API credentials in `appsettings.json`

2. **Database Migration** (if not already applied)
   - OtpCode table should already exist
   - Run migrations to ensure `Used` column exists: `ALTER TABLE tbl_OtpCode ADD Used BIT DEFAULT 0;`

3. **Cleanup Task** (optional but recommended)
   - Set up periodic cleanup via Hangfire or hosted service
   - Call `IOtpService.CleanupExpiredOtpsAsync()` every 5-10 minutes

4. **Frontend Integration**
   - Use the 3 endpoints above for customer OTP verification during booking
   - Show remaining attempts count to user
   - Implement countdown timer for 2-minute expiry

## Files Created
- `DTOs/Otp/RequestOtpDto.cs`
- `DTOs/Otp/VerifyOtpDto.cs`
- `DTOs/Otp/OtpRequestResponseDto.cs`
- `DTOs/Otp/OtpVerificationResponseDto.cs`
- `Services/Sms/ISmsService.cs`
- `Services/Sms/SmsService.cs`
- `Services/Otp/IOtpService.cs`
- `Services/Otp/OtpService.cs`
- `Controllers/OtpController.cs`
- `Program.cs` (updated)
- `DOCS/OTP_SERVICE.md` (detailed documentation)

Build Status: ? **Successful**
