using booking_backend.DTOs.Customers;

namespace booking_backend.Services.Customers;

/// Service interface for managing customers
public interface ICustomerService
{
    /// Creates a new customer
    Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto request, CancellationToken cancellationToken = default);

    /// Retrieves a customer by ID
    Task<CustomerDto?> GetCustomerByIdAsync(int customerId, CancellationToken cancellationToken = default);

    /// Retrieves all customers
    Task<IEnumerable<CustomerDto>> GetAllCustomersAsync(CancellationToken cancellationToken = default);

    /// Updates an existing customer
    Task<CustomerDto?> UpdateCustomerAsync(int customerId, UpdateCustomerDto request, CancellationToken cancellationToken = default);

    /// Deletes a customer
    Task<bool> DeleteCustomerAsync(int customerId, CancellationToken cancellationToken = default);
}
