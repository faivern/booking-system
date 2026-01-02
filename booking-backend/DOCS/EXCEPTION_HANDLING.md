# Global Exception Handling Implementation

## Overview

A centralized exception handling middleware has been added to the booking system. This middleware catches all unhandled exceptions and returns a standardized error response to the client.

## Features

? **Centralized Error Handling** - All exceptions caught in one place  
? **Standardized Responses** - Consistent error format across all endpoints  
? **HTTP Status Codes** - Proper status codes (400, 404, 409, 500, etc.)  
? **Error Codes** - Machine-readable error codes for frontend  
? **Security** - Stack traces not exposed to clients in production  
? **Logging** - Full exception details logged server-side  
? **Trace IDs** - Every error includes trace ID for debugging  

## Architecture

```
Request ? Middleware ? Controllers/Services
                           ? Exception
                      Exception Handling
                           ?
                      Log Exception
                           ?
                      Return Standardized Response
```

## Exception Types

### 1. **ValidationException**
```csharp
throw new ValidationException("Field validation failed", new Dictionary<string, string[]>
{
    { "Email", new[] { "Email format is invalid" } }
});
```

**Response (400):**
```json
{
    "success": false,
    "message": "Field validation failed",
    "errorCode": "VALIDATION_ERROR",
    "errors": {
        "Email": ["Email format is invalid"]
    },
    "traceId": "0HN1GHEV5NABQ:00000001"
}
```

### 2. **ResourceNotFoundException**
```csharp
throw new ResourceNotFoundException("Booking with ID 123 not found");
```

**Response (404):**
```json
{
    "success": false,
    "message": "Booking with ID 123 not found",
    "errorCode": "NOT_FOUND",
    "traceId": "0HN1GHEV5NABQ:00000002"
}
```

### 3. **ConflictException**
```csharp
throw new ConflictException("Time slot is not available for this service");
```

**Response (409):**
```json
{
    "success": false,
    "message": "Time slot is not available for this service",
    "errorCode": "CONFLICT",
    "traceId": "0HN1GHEV5NABQ:00000003"
}
```

### 4. **UnauthorizedException**
```csharp
throw new UnauthorizedException("Admin session expired");
```

**Response (401):**
```json
{
    "success": false,
    "message": "Admin session expired",
    "errorCode": "UNAUTHORIZED",
    "traceId": "0HN1GHEV5NABQ:00000004"
}
```

### 5. **ArgumentException** (Built-in)
```csharp
throw new ArgumentException("EndTime must be after StartTime");
```

**Response (400):**
```json
{
    "success": false,
    "message": "EndTime must be after StartTime",
    "errorCode": "INVALID_ARGUMENT",
    "traceId": "0HN1GHEV5NABQ:00000005"
}
```

### 6. **Generic/Unhandled Exception**
```csharp
throw new Exception("Database connection failed");
```

**Response (500) - Development:**
```json
{
    "success": false,
    "message": "Database connection failed",
    "errorCode": "INTERNAL_ERROR",
    "traceId": "0HN1GHEV5NABQ:00000006"
}
```

**Response (500) - Production:**
```json
{
    "success": false,
    "message": "An unexpected error occurred",
    "errorCode": "INTERNAL_ERROR",
    "traceId": "0HN1GHEV5NABQ:00000006"
}
```

## Standard Error Response Format

All error responses follow this format:

```csharp
public record ErrorResponse(
    bool Success,                           // Always false for errors
    string Message,                         // User-friendly message
    string? ErrorCode,                      // Machine-readable code
    Dictionary<string, string[]>? Errors,   // Detailed validation errors (optional)
    string? TraceId);                       // Correlation ID for debugging
```

## Error Codes Reference

| Code | HTTP Status | Meaning |
|------|-------------|---------|
| VALIDATION_ERROR | 400 | Input validation failed |
| INVALID_ARGUMENT | 400 | Invalid parameter value |
| NOT_FOUND | 404 | Resource doesn't exist |
| CONFLICT | 409 | Business logic conflict |
| UNAUTHORIZED | 401 | Not authenticated/authorized |
| INTERNAL_ERROR | 500 | Server error |
| GENERAL_ERROR | 400 | Generic application error |

## Development vs Production

### Development Mode
- Stack traces included in error messages
- Detailed exception information visible
- Useful for debugging

### Production Mode
- Generic error messages
- Stack traces hidden from client
- Full details logged server-side only
- Protects sensitive information

## Logging

All exceptions are logged with full details:

```
[ERROR] Unhandled exception occurred
System.InvalidOperationException: Time slot is not available
   at booking_backend.Services.Bookings.BookingService.CreateBookingAsync(CreateBookingDto request, CancellationToken cancellationToken)
   at booking_backend.Controllers.BookingsController.CreateBooking(CreateBookingDto request, CancellationToken cancellationToken)
   ...
```

Check application logs to see full stack traces and debugging information.

## Files Created

1. **DTOs/Common/ErrorResponse.cs** - Standard error response DTO
2. **Exceptions/ApplicationException.cs** - Custom exception types
3. **Middleware/ExceptionHandlingMiddleware.cs** - Exception handling middleware

## Files Modified

1. **Program.cs** - Added middleware registration

## Usage Examples

### Example 1: Validation Error
```csharp
public async Task<BookingDto> CreateBookingAsync(CreateBookingDto request, CancellationToken cancellationToken)
{
    if (request.EndTime <= request.StartTime)
    {
        throw new ArgumentException("EndTime must be after StartTime");
    }
    // ...
}
```

### Example 2: Not Found
```csharp
var booking = await _context.Bookings.FindAsync(bookingId);
if (booking == null)
{
    throw new ResourceNotFoundException($"Booking {bookingId} not found");
}
```

### Example 3: Conflict
```csharp
var hasConflict = await CheckTimeSlotConflict(booking);
if (hasConflict)
{
    throw new ConflictException("Time slot is already booked");
}
```

### Example 4: Validation with Errors
```csharp
var errors = new Dictionary<string, string[]>();

if (string.IsNullOrWhiteSpace(request.Email))
{
    errors["Email"] = new[] { "Email is required" };
}

if (request.Price < 0)
{
    errors["Price"] = new[] { "Price cannot be negative" };
}

if (errors.Any())
{
    throw new ValidationException("Validation failed", errors);
}
```

## Testing Exception Handling

### Test with cURL

**Valid Request (Should succeed):**
```bash
curl -X POST http://localhost:5000/api/bookings \
  -H "Content-Type: application/json" \
  -d '{
    "businessId": 1,
    "serviceId": 1,
    "customerId": 1,
    "startTime": "2026-01-15T10:00:00Z",
    "endTime": "2026-01-15T11:00:00Z"
  }'
```

**Invalid Request (Time conflict):**
```bash
curl -X POST http://localhost:5000/api/bookings \
  -H "Content-Type: application/json" \
  -d '{
    "businessId": 1,
    "serviceId": 1,
    "customerId": 1,
    "startTime": "2026-01-15T11:00:00Z",
    "endTime": "2026-01-15T10:00:00Z"
  }'
```

**Expected Response (400):**
```json
{
    "success": false,
    "message": "EndTime must be after StartTime",
    "errorCode": "INVALID_ARGUMENT",
    "traceId": "0HN1GHEV5NABQ:00000001"
}
```

## Benefits

1. **Consistency** - All errors return same format
2. **Security** - Stack traces hidden in production
3. **Debugging** - Trace IDs for correlation
4. **Frontend** - Can handle errors uniformly
5. **Logging** - Full details captured server-side
6. **Maintainability** - Centralized error logic

## Next Steps

1. Test exception handling manually
2. Update services to use custom exceptions (optional)
3. Implement Swagger documentation
4. Start frontend development

## Migration Guide

### From Manual Exception Handling
**Before:**
```csharp
catch (ArgumentException ex)
{
    _logger.LogWarning(ex, "Validation error");
    return BadRequest(new { message = ex.Message });
}
```

**After (with middleware):**
```csharp
// Just throw - middleware handles it
throw new ArgumentException("Validation failed");
```

The middleware automatically:
- Catches the exception
- Logs it with full details
- Returns standardized response
- Sets correct HTTP status code

