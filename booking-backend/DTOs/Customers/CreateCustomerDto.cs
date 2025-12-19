namespace booking_backend.DTOs.Customers;

/// <summary>
/// DTO for creating a new customer
/// </summary>
public record CreateCustomerDto(
    string FirstName,
    string LastName,
    string? Email,
    string? Phone);
