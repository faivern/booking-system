using booking_backend.DTOs.Services;

namespace booking_backend.Services.Services;

/// <summary>
/// Service interface for managing services
/// </summary>
public interface IServiceService
{
    /// <summary>
    /// Creates a new service
    /// </summary>
    Task<ServiceDto> CreateServiceAsync(CreateServiceDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a service by ID
    /// </summary>
    Task<ServiceDto?> GetServiceByIdAsync(int serviceId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all services for a business
    /// </summary>
    Task<IEnumerable<ServiceDto>> GetBusinessServicesAsync(int businessId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing service
    /// </summary>
    Task<ServiceDto?> UpdateServiceAsync(int serviceId, UpdateServiceDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a service
    /// </summary>
    Task<bool> DeleteServiceAsync(int serviceId, CancellationToken cancellationToken = default);
}
