using Microsoft.EntityFrameworkCore;
using booking_backend.Data;
using booking_backend.DTOs.Customers;
using booking_backend.Models;

namespace booking_backend.Services.Customers;

/// Service for managing customers
public class CustomerService : ICustomerService
{
    private readonly BookingSystemDbContext _context;

    public CustomerService(BookingSystemDbContext context)
    {
        _context = context;
    }

    /// Creates a new customer
    public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto request, CancellationToken cancellationToken = default)
    {
        // Validate required fields
        if (string.IsNullOrWhiteSpace(request.FirstName))
        {
            throw new ArgumentException("First name is required");
        }

        if (string.IsNullOrWhiteSpace(request.LastName))
        {
            throw new ArgumentException("Last name is required");
        }

        var customer = new Customer
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Phone = request.Phone
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync(cancellationToken);

        return MapToDto(customer);
    }

    /// Retrieves a customer by ID
    public async Task<CustomerDto?> GetCustomerByIdAsync(int customerId, CancellationToken cancellationToken = default)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.CustomerId == customerId, cancellationToken);

        return customer == null ? null : MapToDto(customer);
    }

    /// Retrieves all customers
    public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync(CancellationToken cancellationToken = default)
    {
        var customers = await _context.Customers
            .OrderBy(c => c.LastName)
            .ThenBy(c => c.FirstName)
            .ToListAsync(cancellationToken);

        return customers.Select(MapToDto);
    }

    /// Updates an existing customer
    public async Task<CustomerDto?> UpdateCustomerAsync(int customerId, UpdateCustomerDto request, CancellationToken cancellationToken = default)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.CustomerId == customerId, cancellationToken);

        if (customer == null)
        {
            return null;
        }

        // Validate required fields
        if (string.IsNullOrWhiteSpace(request.FirstName))
        {
            throw new ArgumentException("First name is required");
        }

        if (string.IsNullOrWhiteSpace(request.LastName))
        {
            throw new ArgumentException("Last name is required");
        }

        customer.FirstName = request.FirstName;
        customer.LastName = request.LastName;
        customer.Email = request.Email;
        customer.Phone = request.Phone;

        _context.Customers.Update(customer);
        await _context.SaveChangesAsync(cancellationToken);

        return MapToDto(customer);
    }

    /// Deletes a customer
    public async Task<bool> DeleteCustomerAsync(int customerId, CancellationToken cancellationToken = default)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.CustomerId == customerId, cancellationToken);

        if (customer == null)
        {
            return false;
        }

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    /// Maps a Customer entity to a CustomerDto
    private static CustomerDto MapToDto(Customer customer)
    {
        return new CustomerDto(
            customer.CustomerId,
            customer.FirstName,
            customer.LastName,
            customer.Email,
            customer.Phone);
    }
}
