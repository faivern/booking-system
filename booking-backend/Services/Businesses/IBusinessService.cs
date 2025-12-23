using booking_backend.DTOs.Businesses;

namespace booking_backend.Services.Businesses;

/// Service interface for managing businesses
public interface IBusinessService
{
    /// Creates a new business
    Task<BusinessDto> CreateBusinessAsync(CreateBusinessDto request, CancellationToken cancellationToken = default);

    /// Retrieves a business by ID
    Task<BusinessDto?> GetBusinessByIdAsync(int businessId, CancellationToken cancellationToken = default);

    /// Retrieves all businesses
    Task<IEnumerable<BusinessDto>> GetAllBusinessesAsync(CancellationToken cancellationToken = default);

    /// Updates an existing business
    Task<BusinessDto?> UpdateBusinessAsync(int businessId, UpdateBusinessDto request, CancellationToken cancellationToken = default);

    /// Deletes a business
    Task<bool> DeleteBusinessAsync(int businessId, CancellationToken cancellationToken = default);
}
