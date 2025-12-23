using booking_backend.DTOs.Customers;

namespace booking_backend.Services.Customers;

/// <summary>
/// Service interface for managing customers
/// </summary>
public interface ICustomerService
{
    /// <summary>
    /// Creates a new customer
    /// </summary>
    Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a customer by ID
    /// </summary>
    Task<CustomerDto?> GetCustomerByIdAsync(int customerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all customers
    /// </summary>
    Task<IEnumerable<CustomerDto>> GetAllCustomersAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing customer
    /// </summary>
    Task<CustomerDto?> UpdateCustomerAsync(int customerId, UpdateCustomerDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a customer
    /// </summary>
    Task<bool> DeleteCustomerAsync(int customerId, CancellationToken cancellationToken = default);
}
