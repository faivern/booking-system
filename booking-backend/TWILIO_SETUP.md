# Twilio SMS Integration - Setup Guide

## Quick Start

### 1. Get Your Twilio Credentials

1. Log in to [Twilio Console](https://www.twilio.com/console)
2. Find these three pieces of information:
   - **Account SID** - Account identifier
   - **Auth Token** - API authentication token
   - **Phone Number** - Your Twilio SMS-enabled phone number (e.g., +15551234567)

### 2. Update appsettings.json

Replace placeholder values in `appsettings.json`:

```json
{
  "Twilio": {
    "AccountSid": "ACxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx",
    "AuthToken": "your_auth_token_here",
    "FromPhoneNumber": "+15551234567"
  }
}
```

### 3. Test in Development

The app is in **Development** mode by default. OTP codes are logged to console:

```
?? SMS Message (DEV MODE) to +15551234567: Your OTP code is: 123456. Valid for 2 minutes. Do not share this code.
```

Use the logged code to test OTP verification.

### 4. Deploy to Production

Set environment variables:

```bash
# Local environment variable
export Twilio__AccountSid=ACxxxxxxxx
export Twilio__AuthToken=your_token
export Twilio__FromPhoneNumber=+15551234567
```

For Azure/Cloud:
- Use App Configuration or Key Vault
- Set as environment variables in deployment config

## What's Included

### Features Implemented ?

1. **OTP SMS Sending**
   - Endpoint: `POST /api/otp/request`
   - Sends 6-digit code via SMS
   - Valid for 2 minutes
   - Works in development (logged) and production (via Twilio)

2. **Booking Reminders**
   - Automatically sends SMS 36 hours before booking
   - Runs every hour in background
   - Includes service name, business name, and appointment time
   - Only sends for confirmed bookings

### Services Created

| Service | Purpose |
|---------|---------|
| `TwilioSmsService` | Sends SMS via Twilio API |
| `BookingReminderService` | Queries upcoming bookings and sends reminders |
| `BookingReminderHostedService` | Runs reminder checks hourly in background |

### Configuration

Twilio settings are in `appsettings.json`:

```json
{
  "Twilio": {
    "AccountSid": "YOUR_ACCOUNT_SID",
    "AuthToken": "YOUR_AUTH_TOKEN", 
    "FromPhoneNumber": "YOUR_TWILIO_PHONE_NUMBER"
  }
}
```

## API Endpoints

### 1. Request OTP
```
POST /api/otp/request
Content-Type: application/json

{
    "phoneNumber": "+15551234567"
}
```

**Success Response (200):**
```json
{
    "success": true,
    "message": "OTP sent successfully to your phone",
    "phoneNumber": "+15551234567",
    "expiresInSeconds": 120
}
```

### 2. Verify OTP
```
POST /api/otp/verify
Content-Type: application/json

{
    "phoneNumber": "+15551234567",
    "code": "123456"
}
```

**Success Response (200):**
```json
{
    "success": true,
    "message": "OTP verified successfully",
    "isValid": true
}
```

## Development vs Production

### Development Environment
- **Mode**: ASPNETCORE_ENVIRONMENT=Development
- **OTP Behavior**: Logged to console, not sent
- **Reminders**: Logged to console, not sent
- **Benefit**: Test without SMS costs

### Production Environment
- **Mode**: ASPNETCORE_ENVIRONMENT=Production
- **OTP Behavior**: Sent via Twilio (costs apply)
- **Reminders**: Sent via Twilio hourly
- **Requirement**: Valid Twilio credentials

## Testing Checklist

- [ ] OTP request returns logged code in development
- [ ] OTP verification works with logged code
- [ ] Twilio credentials added to appsettings.json
- [ ] Booking created with customer phone number
- [ ] Check logs for "Reminder sent" messages

## Troubleshooting

| Problem | Solution |
|---------|----------|
| "Twilio is not configured" message | Check appsettings.json has AccountSid, AuthToken, FromPhoneNumber |
| SMS not received | Verify phone number format is +1234567890 |
| 401 Unauthorized error | Verify AccountSid and AuthToken are correct |
| OTP not logging in dev | Ensure ASPNETCORE_ENVIRONMENT=Development |

## Files Modified

- ? `booking-backend.csproj` - Added Twilio NuGet package
- ? `Services/Sms/TwilioSmsService.cs` - Twilio SMS implementation
- ? `Services/Sms/IBookingReminderService.cs` - Reminder service
- ? `Services/Sms/BookingReminderHostedService.cs` - Background service
- ? `appsettings.json` - Twilio configuration
- ? `Program.cs` - Service registration

## Next Steps

1. Create Twilio account: https://www.twilio.com/
2. Get Account SID, Auth Token, Phone Number
3. Update appsettings.json
4. Test OTP in development (check logs)
5. Deploy to production with environment variables
6. Monitor logs for SMS delivery

## Support

- Twilio Docs: https://www.twilio.com/docs/sms
- Twilio Support: https://support.twilio.com/
- View Sent Messages: Twilio Console ? Messages
