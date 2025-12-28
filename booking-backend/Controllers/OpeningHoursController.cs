using Microsoft.AspNetCore.Mvc;
using booking_backend.DTOs.OpeningHours;
using booking_backend.Services.OpeningHours;

namespace booking_backend.Controllers;

/// <summary>
/// API controller for managing opening hours
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class OpeningHoursController : ControllerBase
{
    private readonly IOpeningHourService _openingHourService;
    private readonly ILogger<OpeningHoursController> _logger;

    public OpeningHoursController(IOpeningHourService openingHourService, ILogger<OpeningHoursController> logger)
    {
        _openingHourService = openingHourService;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new opening hour for a business
    /// </summary>
    /// <param name="request">The opening hour creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created opening hour</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateOpeningHour(
        [FromBody] CreateOpeningHourDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _openingHourService.CreateOpeningHourAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetOpeningHourById), new { id = result.OpeningHourId }, result);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Validation error while creating opening hour");
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Business logic error while creating opening hour");
            return NotFound(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves an opening hour by ID
    /// </summary>
    /// <param name="id">The opening hour ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The opening hour details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOpeningHourById(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var result = await _openingHourService.GetOpeningHourByIdAsync(id, cancellationToken);
        
        if (result == null)
        {
            return NotFound(new { message = $"Opening hour {id} not found" });
        }

        return Ok(result);
    }

    /// <summary>
    /// Retrieves all opening hours for a business
    /// </summary>
    /// <param name="businessId">The business ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of business opening hours</returns>
    [HttpGet("business/{businessId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBusinessOpeningHours(
        [FromRoute] int businessId,
        CancellationToken cancellationToken)
    {
        var result = await _openingHourService.GetBusinessOpeningHoursAsync(businessId, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Updates an existing opening hour
    /// </summary>
    /// <param name="id">The opening hour ID</param>
    /// <param name="request">The opening hour update request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated opening hour</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateOpeningHour(
        [FromRoute] int id,
        [FromBody] UpdateOpeningHourDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _openingHourService.UpdateOpeningHourAsync(id, request, cancellationToken);
            
            if (result == null)
            {
                return NotFound(new { message = $"Opening hour {id} not found" });
            }

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Validation error while updating opening hour");
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Deletes an opening hour
    /// </summary>
    /// <param name="id">The opening hour ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>No content</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteOpeningHour(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var result = await _openingHourService.DeleteOpeningHourAsync(id, cancellationToken);
        
        if (!result)
        {
            return NotFound(new { message = $"Opening hour {id} not found" });
        }

        return NoContent();
    }
}
