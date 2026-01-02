# Swagger/OpenAPI Setup Guide

## Overview

Swagger/OpenAPI has been configured for the Booking System API. This provides interactive API documentation and testing capabilities.

## Accessing Swagger UI

### Local Development
```
http://localhost:5000/swagger
```

Or if running on HTTPS:
```
https://localhost:5001/swagger
```

## Features

? **View All Endpoints** - See all 25 API endpoints organized by controller  
? **Test Endpoints** - Send requests directly from Swagger UI  
? **View Schemas** - See request/response data structures  
? **Try It Out** - Execute API calls without external tools  
? **Curl Commands** - Copy auto-generated curl commands  

## Endpoint Organization

Swagger groups endpoints by controller:

### **Admin Controller** (3 endpoints)
- `POST /api/admin/login` - Admin login
- `GET /api/admin/validate-session` - Check session validity
- `POST /api/admin/logout` - Admin logout

### **Bookings Controller** (5 endpoints)
- `POST /api/bookings` - Create booking
- `GET /api/bookings/{id}` - Get booking by ID
- `GET /api/bookings/customer/{customerId}` - Get customer bookings
- `GET /api/bookings/business/{businessId}` - Get business bookings
- `PUT /api/bookings/{id}` - Update booking
- `DELETE /api/bookings/{id}` - Delete booking

### **Services Controller** (5 endpoints)
- `POST /api/services` - Create service
- `GET /api/services/{id}` - Get service by ID
- `GET /api/services/business/{businessId}` - Get business services
- `PUT /api/services/{id}` - Update service
- `DELETE /api/services/{id}` - Delete service

### **Customers Controller** (5 endpoints)
- `POST /api/customers` - Create customer
- `GET /api/customers/{id}` - Get customer by ID
- `GET /api/customers` - Get all customers
- `PUT /api/customers/{id}` - Update customer
- `DELETE /api/customers/{id}` - Delete customer

### **Businesses Controller** (5 endpoints)
- `POST /api/businesses` - Create business
- `GET /api/businesses/{id}` - Get business by ID
- `GET /api/businesses` - Get all businesses
- `PUT /api/businesses/{id}` - Update business
- `DELETE /api/businesses/{id}` - Delete business

### **OpeningHours Controller** (5 endpoints)
- `POST /api/openinghours` - Create opening hour
- `GET /api/openinghours/{id}` - Get opening hour by ID
- `GET /api/openinghours/business/{businessId}` - Get business opening hours
- `PUT /api/openinghours/{id}` - Update opening hour
- `DELETE /api/openinghours/{id}` - Delete opening hour

### **OTP Controller** (3 endpoints)
- `POST /api/otp/request` - Request OTP code
- `POST /api/otp/verify` - Verify OTP code
- `GET /api/otp/remaining-attempts/{phoneNumber}` - Get remaining OTP attempts

## How to Test an Endpoint

### Example: Create a Booking

1. **Open Swagger UI**
   - Navigate to `http://localhost:5000/swagger`

2. **Expand Bookings Section**
   - Click on "Bookings" to expand

3. **Find POST /api/bookings**
   - Click on the endpoint

4. **Click "Try It Out"**
   - Button appears on the right

5. **Fill in Request Body**
```json
{
  "businessId": 1,
  "serviceId": 1,
  "customerId": 1,
  "startTime": "2026-01-15T10:00:00Z",
  "endTime": "2026-01-15T11:00:00Z"
}
```

6. **Click "Execute"**
   - Request is sent to backend

7. **View Response**
   - See status code, headers, and response body
   - If successful (201): See created booking data
   - If failed (400/409): See error message and error code

## Error Responses

All errors follow standardized format:

```json
{
  "success": false,
  "message": "User-friendly error message",
  "errorCode": "ERROR_CODE",
  "errors": {
    "fieldName": ["Validation error 1", "Validation error 2"]
  },
  "traceId": "0HN1GHEV5NABQ:00000001"
}
```

### Common Error Codes

| Code | Status | Meaning |
|------|--------|---------|
| VALIDATION_ERROR | 400 | Input validation failed |
| INVALID_ARGUMENT | 400 | Invalid parameter |
| NOT_FOUND | 404 | Resource not found |
| CONFLICT | 409 | Time slot conflict |
| UNAUTHORIZED | 401 | Not authenticated |
| INTERNAL_ERROR | 500 | Server error |

## Testing Workflow

### 1. Create a Business
```
POST /api/businesses
{
  "name": "Jane's Salon",
  "description": "Professional hair salon",
  "email": "jane@salon.com",
  "phone": "+1234567890"
}
```

### 2. Create a Service
```
POST /api/services
{
  "businessId": 1,  // From previous response
  "name": "Hair Cutting",
  "description": "Professional hair cut",
  "durationMinutes": 60,
  "price": 50
}
```

### 3. Create a Customer
```
POST /api/customers
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john@example.com",
  "phone": "+1234567890"
}
```

### 4. Create a Booking
```
POST /api/bookings
{
  "businessId": 1,
  "serviceId": 1,
  "customerId": 1,
  "startTime": "2026-01-15T10:00:00Z",
  "endTime": "2026-01-15T11:00:00Z"
}
```

### 5. Verify Booking
```
GET /api/bookings/{bookingId}
```

## Status Codes

| Code | Meaning | Example |
|------|---------|---------|
| 200 | OK - Request succeeded | GET successful |
| 201 | Created - Resource created | POST successful |
| 204 | No Content - Resource deleted | DELETE successful |
| 400 | Bad Request - Invalid input | Validation error |
| 401 | Unauthorized - Not authenticated | Missing auth |
| 404 | Not Found - Resource doesn't exist | ID not found |
| 409 | Conflict - Business logic error | Time slot taken |
| 500 | Internal Server Error | Unexpected error |

## Useful Features

### Copy Curl Command
1. Execute an endpoint
2. Click "Curl" tab in response
3. Copy the curl command
4. Run in terminal: `curl [command]`

### Request Headers
- **Content-Type**: Already set to application/json
- **Accept**: Already set to application/json
- **Cookies**: Session cookies from login are automatically sent

### Response Inspection
- **Status Code**: HTTP response status
- **Headers**: Response headers (Content-Type, etc.)
- **Body**: Full response data
- **Raw**: Raw response view

## Tips for Testing

? **Test in Order**: Create business ? service ? customer ? booking  
? **Use Real IDs**: Note IDs from responses for related operations  
? **Check Error Codes**: Use errorCode for frontend error handling  
? **Copy Curl Commands**: Share API calls with team members  
? **Check Timestamps**: Verify ISO 8601 format for dates  
? **Validate Phone Numbers**: Use format +1234567890  

## Troubleshooting

### Swagger UI not loading
- Ensure app is running: `dotnet run`
- Check URL: `http://localhost:5000/swagger` (port may vary)
- Check console for errors

### Cannot execute requests
- Ensure backend is running
- Check CORS configuration in Program.cs
- Verify Content-Type is application/json

### Validation errors
- Check error response for detailed field errors
- Review field requirements in error message
- Use example values from this guide

### 500 errors
- Check application logs for stack trace
- Verify database connection
- Ensure all foreign keys are valid

## Environment-Specific Access

### Development (Local)
```
http://localhost:5000/swagger
https://localhost:5001/swagger
```

### Production (if deployed)
```
https://api.bookingsystem.com/swagger
```

**Note**: Swagger is disabled in production by default. Enable only if needed for testing.

## Next Steps

1. ? Start the backend: `dotnet run`
2. ? Open browser: `http://localhost:5000/swagger`
3. ? Test endpoints using "Try It Out"
4. ? Verify error handling with invalid requests
5. ? Note endpoint URLs for frontend integration

