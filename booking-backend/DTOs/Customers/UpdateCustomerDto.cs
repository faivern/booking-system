namespace booking_backend.DTOs.Customers;

/// <summary>
/// DTO for updating an existing customer
/// </summary>
public record UpdateCustomerDto(
    string FirstName,
    string LastName,
    string? Email,
    string? Phone);
