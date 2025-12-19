namespace booking_backend.DTOs.Customers;

/// <summary>
/// DTO for returning customer data
/// </summary>
public record CustomerDto(
    int CustomerId,
    string FirstName,
    string LastName,
    string? Email,
    string? Phone);
