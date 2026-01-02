using Microsoft.AspNetCore.Mvc;
using booking_backend.DTOs.Auth;
using booking_backend.Services.Auth;

namespace booking_backend.Controllers;

/// <summary>
/// API controller for simple admin authentication
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IAdminAuthService _authService;
    private readonly ILogger<AdminController> _logger;
    private const string SessionCookieName = "AdminSession";

    /// <summary>
    /// Initializes a new instance of the <see cref="AdminController"/> class.
    /// </summary>
    /// <param name="authService">The admin authentication service</param>
    /// <param name="logger">The logger instance</param>
    public AdminController(IAdminAuthService authService, ILogger<AdminController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Logs in an admin user
    /// </summary>
    /// <param name="request">The login request</param>
    /// <returns>Login response with session ID</returns>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Login([FromBody] AdminLoginDto request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new AdminLoginResponseDto(false, "Username and password are required", null));
            }

            var sessionId = _authService.Login(request.Username, request.Password);

            if (string.IsNullOrEmpty(sessionId))
            {
                return Unauthorized(new AdminLoginResponseDto(false, "Invalid username or password", null));
            }

            // Set session cookie
            Response.Cookies.Append(SessionCookieName, sessionId, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(8)
            });

            return Ok(new AdminLoginResponseDto(true, "Login successful", sessionId));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during admin login");
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new AdminLoginResponseDto(false, "An error occurred during login", null));
        }
    }

    /// <summary>
    /// Validates the current admin session
    /// </summary>
    /// <returns>Validation result</returns>
    [HttpGet("validate-session")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult ValidateSession()
    {
        if (!Request.Cookies.TryGetValue(SessionCookieName, out var sessionId) || string.IsNullOrEmpty(sessionId))
        {
            return Unauthorized(new { valid = false, message = "No active session" });
        }

        var isValid = _authService.ValidateSession(sessionId);

        if (!isValid)
        {
            // Clear expired session cookie
            Response.Cookies.Delete(SessionCookieName);
            return Unauthorized(new { valid = false, message = "Session expired" });
        }

        var expiryTime = _authService.GetSessionExpiry(sessionId);

        return Ok(new 
        { 
            valid = true, 
            message = "Session is active",
            expiresAt = expiryTime 
        });
    }

    /// <summary>
    /// Logs out the current admin session
    /// </summary>
    /// <returns>Logout response</returns>
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Logout()
    {
        if (Request.Cookies.TryGetValue(SessionCookieName, out var sessionId) && !string.IsNullOrEmpty(sessionId))
        {
            _authService.Logout(sessionId);
        }

        // Clear session cookie
        Response.Cookies.Delete(SessionCookieName);

        return Ok(new { message = "Logout successful" });
    }
}
