using booking_backend.DTOs.Services;

namespace booking_backend.Services.Services;

/// Service interface for managing services
public interface IServiceService
{
    /// Creates a new service
    Task<ServiceDto> CreateServiceAsync(CreateServiceDto request, CancellationToken cancellationToken = default);

    /// Retrieves a service by ID
    Task<ServiceDto?> GetServiceByIdAsync(int serviceId, CancellationToken cancellationToken = default);

    /// Retrieves all services for a business
    Task<IEnumerable<ServiceDto>> GetBusinessServicesAsync(int businessId, CancellationToken cancellationToken = default);

    /// Updates an existing service
    Task<ServiceDto?> UpdateServiceAsync(int serviceId, UpdateServiceDto request, CancellationToken cancellationToken = default);

    /// Deletes a service
    Task<bool> DeleteServiceAsync(int serviceId, CancellationToken cancellationToken = default);
}