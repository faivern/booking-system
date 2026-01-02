# Twilio SMS Integration Guide

## Overview

This guide explains how to set up and use the Twilio SMS integration in your booking system. The system sends:
1. **OTP Codes** - 6-digit verification codes for customers during booking
2. **Booking Reminders** - Automated SMS 36 hours before scheduled appointments

## Setup Instructions

### Step 1: Get Twilio Credentials

1. Visit [Twilio Console](https://www.twilio.com/console)
2. Sign in to your account
3. Locate your credentials:
   - **Account SID** - Found in the main dashboard
   - **Auth Token** - Found in the main dashboard (click "Show" to reveal)
   - **Phone Number** - Your Twilio phone number (starts with +1 for US, e.g., +15551234567)

### Step 2: Update Configuration

Update `appsettings.json` with your Twilio credentials:

```json
{
  "Twilio": {
    "AccountSid": "ACxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx",
    "AuthToken": "your_auth_token_here",
    "FromPhoneNumber": "+15551234567"
  }
}
```

**Important:** Never commit these credentials to version control. Use environment variables in production:

```json
{
  "Twilio": {
    "AccountSid": "${TWILIO_ACCOUNT_SID}",
    "AuthToken": "${TWILIO_AUTH_TOKEN}",
    "FromPhoneNumber": "${TWILIO_PHONE_NUMBER}"
  }
}
```

### Step 3: Environment Variables (Production)

Set environment variables on your hosting platform:

**Azure:**
```bash
az webapp config appsettings set --resource-group myResourceGroup --name myApp --settings Twilio__AccountSid="ACxxxxxxxx" Twilio__AuthToken="your_token" Twilio__FromPhoneNumber="+15551234567"
```

**Docker/Local:**
```bash
export Twilio__AccountSid=ACxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
export Twilio__AuthToken=your_auth_token_here
export Twilio__FromPhoneNumber=+15551234567
```

## Features

### 1. OTP Code Sending

**Endpoint:** `POST /api/otp/request`

When a customer requests an OTP:
- System generates 6-digit code
- Twilio sends SMS with code
- Code valid for 2 minutes

**Request:**
```json
{
    "phoneNumber": "+15551234567"
}
```

**Response:**
```json
{
    "success": true,
    "message": "OTP sent successfully to your phone",
    "phoneNumber": "+15551234567",
    "expiresInSeconds": 120
}
```

**SMS Content:**
```
Your OTP code is: 123456. Valid for 2 minutes. Do not share this code.
```

### 2. Booking Reminders

**Automatic Reminder Sending**

- Runs every hour in the background
- Checks for bookings scheduled in ~36 hours
- Sends SMS reminder to customer
- Only sends for "Confirmed" bookings

**SMS Content Example:**
```
Reminder: You have a booking for Hair Cutting at Jane's Salon on December 15 at 02:30 PM. 
If you need to cancel or reschedule, please contact the business. See you soon!
```

**Configuration:**
- Reminder Window: 35.5-36.5 hours before booking
- Check Interval: Every 1 hour
- Requirement: Customer must have phone number in booking

## Development Mode

In **Development** environment, SMS messages are **logged instead of sent**:

**Console Output:**
```
?? SMS Message (DEV MODE) to +15551234567: Your OTP code is: 123456. Valid for 2 minutes. Do not share this code.
```

This allows testing without consuming Twilio credits.

## Production Mode

In **Production** environment:
- SMS messages are sent via Twilio API
- Requires valid Twilio credentials
- Costs apply per message sent

## Architecture

### Services

#### TwilioSmsService
- Implements `ISmsService` interface
- Sends SMS via Twilio REST API
- Handles both OTP and reminder messages
- Validates phone numbers before sending
- Logs all SMS activity

#### BookingReminderService
- Implements `IBookingReminderService` interface
- Queries upcoming bookings
- Builds personalized reminder messages
- Calls TwilioSmsService to send reminders

#### BookingReminderHostedService
- Background service that runs on app startup
- Calls BookingReminderService every hour
- Handles errors gracefully with retry logic

## Error Handling

The system handles various error scenarios:

| Scenario | Behavior |
|----------|----------|
| Invalid phone number | Logs warning, returns false |
| Twilio not configured | Logs error, returns false in production |
| Network error | Logs error, returns false |
| Invalid credentials | Twilio API returns error, logged |
| Missing database connection | BookingReminderService logs error |

## Testing

### Test OTP in Development

1. Ensure `ASPNETCORE_ENVIRONMENT=Development`
2. Call `POST /api/otp/request` with your phone number
3. Check application logs for the generated OTP
4. Use that OTP to test `/api/otp/verify`

### Test Reminders (Manual)

```csharp
// In a test controller or endpoint
var reminderService = _serviceProvider.GetRequiredService<IBookingReminderService>();
var remindersSent = await reminderService.SendUpcomingBookingRemindersAsync();
Console.WriteLine($"Reminders sent: {remindersSent}");
```

### Integration Tests

```csharp
[TestMethod]
public async Task TestOtpViaSmsMock()
{
    // Mock ISmsService
    var mockSmsService = new Mock<ISmsService>();
    mockSmsService
        .Setup(s => s.SendSmsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(true);

    var otpService = new OtpService(_context, mockSmsService.Object, _logger);
    var result = await otpService.GenerateAndSendOtpAsync("+15551234567");

    Assert.IsTrue(result.Success);
    mockSmsService.Verify(
        s => s.SendSmsAsync("+15551234567", It.IsAny<string>(), It.IsAny<CancellationToken>()),
        Times.Once);
}
```

## Monitoring

### Log Messages to Monitor

**OTP Sending:**
```
SMS sent successfully to {phoneNumber}. MessageSid: {id}, Status: {status}
```

**Reminder Sending:**
```
Reminder sent for booking {bookingId} to {phone}
Failed to send reminder for booking {bookingId} to {phone}
```

**Configuration Issues:**
```
Twilio SMS service is not properly configured. SMS messages will not be sent.
Twilio is not configured. Cannot send SMS to {phoneNumber}
```

### Metrics to Track

1. **OTP Success Rate** - Count successful vs failed OTP sends
2. **Reminder Delivery** - Track how many reminders are sent per cycle
3. **Customer Response** - Track booking cancellations after reminders
4. **Error Rate** - Monitor API errors and network issues

## Troubleshooting

### SMS Not Sending

**Symptom:** Messages not received, but no errors

**Solutions:**
1. Verify credentials in `appsettings.json`
2. Check ASPNETCORE_ENVIRONMENT is "Production"
3. Verify phone number format: `+1234567890`
4. Check Twilio account balance
5. Review Twilio console logs for delivery status

### Configuration Not Found

**Symptom:** "Twilio is not configured" warning

**Solution:**
1. Ensure appsettings.json contains Twilio section
2. Verify JSON syntax is valid
3. For environment variables, use `Twilio__AccountSid` format (double underscore)

### 401 Unauthorized

**Symptom:** "Twilio API error" in logs

**Solutions:**
1. Verify AccountSid is correct
2. Verify AuthToken is correct
3. Check token has not been regenerated in Twilio console
4. Ensure no extra spaces in credentials

### Phone Number Issues

**Symptom:** "Invalid phone number format" warning

**Solutions:**
1. Use E.164 format: `+1234567890`
2. For US numbers: `+1` + 10-digit number
3. For international: `+` + country code + number
4. Remove spaces, dashes, or parentheses

## Cost Considerations

- **OTP SMS:** ~$0.0075 per message (US)
- **Reminder SMS:** ~$0.0075 per message (US)
- **Prices vary by country**
- **Check Twilio pricing page for your region**

Example costs:
- 1000 OTPs: ~$7.50
- 1000 reminders: ~$7.50
- Monthly with 2000 users: ~$15/month

## Security Best Practices

1. ? Never commit credentials to version control
2. ? Use environment variables in production
3. ? Validate phone numbers before sending
4. ? Log all SMS activity for audit trails
5. ? Use HTTPS for all API calls (automatically with .NET)
6. ? Rotate Auth Token periodically
7. ? Monitor for unusual activity

## Future Enhancements

1. **SMS Templates** - Customize OTP and reminder messages
2. **Multi-Language** - Send reminders in customer's preferred language
3. **Two-Way SMS** - Allow customers to reply to cancel/reschedule
4. **Delivery Reports** - Track delivery status in database
5. **Scheduled Sending** - Queue messages for off-peak times
6. **Failed Retry Logic** - Automatically retry failed messages
7. **Webhook Notifications** - Receive delivery and error callbacks from Twilio

## Support

- **Twilio Support:** https://support.twilio.com/
- **Twilio Docs:** https://www.twilio.com/docs/
- **API Status:** https://status.twilio.com/

## Files Modified/Created

- ? `Services/Sms/TwilioSmsService.cs` - Main Twilio integration
- ? `Services/Sms/IBookingReminderService.cs` - Reminder service interface & implementation
- ? `Services/Sms/BookingReminderHostedService.cs` - Background reminder service
- ? `appsettings.json` - Configuration template
- ? `Program.cs` - Service registration

## Next Steps

1. Create Twilio account at https://www.twilio.com/
2. Get Account SID, Auth Token, and Phone Number
3. Update `appsettings.json` with credentials
4. Test in Development mode (logs only)
5. Deploy to Production with environment variables
6. Monitor logs and SMS delivery
