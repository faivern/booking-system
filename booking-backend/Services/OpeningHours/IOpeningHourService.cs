using booking_backend.DTOs.OpeningHours;

namespace booking_backend.Services.OpeningHours;

/// <summary>
/// Service interface for managing opening hours
/// </summary>
public interface IOpeningHourService
{
    /// <summary>
    /// Creates a new opening hour for a business
    /// </summary>
    /// <param name="request">The opening hour creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created opening hour</returns>
    Task<OpeningHourDto> CreateOpeningHourAsync(CreateOpeningHourDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves an opening hour by ID
    /// </summary>
    /// <param name="openingHourId">The opening hour ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The opening hour details or null if not found</returns>
    Task<OpeningHourDto?> GetOpeningHourByIdAsync(int openingHourId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all opening hours for a business, sorted by day and start time
    /// </summary>
    /// <param name="businessId">The business ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of opening hours for the business</returns>
    Task<IEnumerable<OpeningHourDto>> GetBusinessOpeningHoursAsync(int businessId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing opening hour
    /// </summary>
    /// <param name="openingHourId">The opening hour ID</param>
    /// <param name="request">The opening hour update request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated opening hour or null if not found</returns>
    Task<OpeningHourDto?> UpdateOpeningHourAsync(int openingHourId, UpdateOpeningHourDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an opening hour
    /// </summary>
    /// <param name="openingHourId">The opening hour ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if deleted successfully, false if not found</returns>
    Task<bool> DeleteOpeningHourAsync(int openingHourId, CancellationToken cancellationToken = default);
}
