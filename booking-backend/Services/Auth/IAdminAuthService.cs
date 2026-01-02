namespace booking_backend.Services.Auth;

/// <summary>
/// Service interface for simple admin authentication
/// </summary>
public interface IAdminAuthService
{
    /// <summary>
    /// Authenticates admin with username and password
    /// </summary>
    string? Login(string username, string password);

    /// <summary>
    /// Validates an active session
    /// </summary>
    bool ValidateSession(string sessionId);

    /// <summary>
    /// Logs out an admin session
    /// </summary>
    void Logout(string sessionId);

    /// <summary>
    /// Gets the session expiry time
    /// </summary>
    DateTime? GetSessionExpiry(string sessionId);
}
