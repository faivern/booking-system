namespace booking_backend.Exceptions;

/// <summary>
/// Base custom exception for application
/// </summary>
public class AppException : Exception
{
    /// <summary>
    /// Gets the error code associated with this exception.
    /// </summary>
    public string ErrorCode { get; }

    /// <summary>
    /// Gets the optional dictionary of validation errors keyed by field name.
    /// </summary>
    public Dictionary<string, string[]>? Errors { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AppException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="errorCode">The error code. Defaults to "GENERAL_ERROR".</param>
    /// <param name="errors">Optional dictionary of validation errors.</param>
    public AppException(string message, string errorCode = "GENERAL_ERROR", Dictionary<string, string[]>? errors = null)
        : base(message)
    {
        ErrorCode = errorCode;
        Errors = errors;
    }
}

/// <summary>
/// Exception for validation errors
/// </summary>
public class ValidationException : AppException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="errors">Optional dictionary of validation errors.</param>
    public ValidationException(string message, Dictionary<string, string[]>? errors = null)
        : base(message, "VALIDATION_ERROR", errors)
    {
    }
}

/// <summary>
/// Exception for resource not found
/// </summary>
public class ResourceNotFoundException : AppException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ResourceNotFoundException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public ResourceNotFoundException(string message)
        : base(message, "NOT_FOUND")
    {
    }
}

/// <summary>
/// Exception for business logic conflicts
/// </summary>
public class ConflictException : AppException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConflictException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public ConflictException(string message)
        : base(message, "CONFLICT")
    {
    }
}

/// <summary>
/// Exception for unauthorized access
/// </summary>
public class UnauthorizedException : AppException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnauthorizedException"/> class.
    /// </summary>
    /// <param name="message">The error message. Defaults to "Unauthorized access".</param>
    public UnauthorizedException(string message = "Unauthorized access")
        : base(message, "UNAUTHORIZED")
    {
    }
}
