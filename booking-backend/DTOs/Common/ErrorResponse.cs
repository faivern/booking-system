namespace booking_backend.DTOs.Common;

/// <summary>
/// Standardized error response for all API exceptions
/// </summary>
public record ErrorResponse(
    bool Success,
    string Message,
    string? ErrorCode,
    Dictionary<string, string[]>? Errors = null,
    string? TraceId = null);
