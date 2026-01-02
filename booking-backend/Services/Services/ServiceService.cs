using Microsoft.EntityFrameworkCore;
using booking_backend.Data;
using booking_backend.DTOs.Services;
using booking_backend.Models;

namespace booking_backend.Services.Services;

/// Service for managing services
public class ServiceService : IServiceService
{
    private readonly BookingSystemDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceService"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public ServiceService(BookingSystemDbContext context)
    {
        _context = context;
    }

    /// Creates a new service
    public async Task<ServiceDto> CreateServiceAsync(CreateServiceDto request, CancellationToken cancellationToken = default)
    {
        // Validate required fields
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ArgumentException("Service name is required");
        }

        if (request.DurationMinutes <= 0)
        {
            throw new ArgumentException("Duration must be greater than 0");
        }

        if (request.Price < 0)
        {
            throw new ArgumentException("Price cannot be negative");
        }

        // Verify business exists
        var business = await _context.Businesses
            .FirstOrDefaultAsync(b => b.BusinessId == request.BusinessId, cancellationToken);

        if (business == null)
        {
            throw new InvalidOperationException($"Business {request.BusinessId} not found");
        }

        var service = new Service
        {
            BusinessId = request.BusinessId,
            Name = request.Name,
            Description = request.Description,
            DurationMinutes = request.DurationMinutes,
            Price = request.Price,
            IsActive = true
        };

        _context.Services.Add(service);
        await _context.SaveChangesAsync(cancellationToken);

        return MapToDto(service, business);
    }

    /// Retrieves a service by ID
    public async Task<ServiceDto?> GetServiceByIdAsync(int serviceId, CancellationToken cancellationToken = default)
    {
        var service = await _context.Services
            .Include(s => s.Business)
            .FirstOrDefaultAsync(s => s.ServiceId == serviceId, cancellationToken);

        return service == null ? null : MapToDto(service, service.Business!);
    }

    /// Retrieves all services for a business
    public async Task<IEnumerable<ServiceDto>> GetBusinessServicesAsync(int businessId, CancellationToken cancellationToken = default)
    {
        var services = await _context.Services
            .Where(s => s.BusinessId == businessId)
            .Include(s => s.Business)
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);

        return services.Select(s => MapToDto(s, s.Business!));
    }

    /// Updates an existing service
    public async Task<ServiceDto?> UpdateServiceAsync(int serviceId, UpdateServiceDto request, CancellationToken cancellationToken = default)
    {
        var service = await _context.Services
            .Include(s => s.Business)
            .FirstOrDefaultAsync(s => s.ServiceId == serviceId, cancellationToken);

        if (service == null)
        {
            return null;
        }

        // Validate required fields
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ArgumentException("Service name is required");
        }

        if (request.DurationMinutes <= 0)
        {
            throw new ArgumentException("Duration must be greater than 0");
        }

        if (request.Price < 0)
        {
            throw new ArgumentException("Price cannot be negative");
        }

        service.Name = request.Name;
        service.Description = request.Description;
        service.DurationMinutes = request.DurationMinutes;
        service.Price = request.Price;
        service.IsActive = request.IsActive;

        _context.Services.Update(service);
        await _context.SaveChangesAsync(cancellationToken);

        return MapToDto(service, service.Business!);
    }

    /// Deletes a service
    public async Task<bool> DeleteServiceAsync(int serviceId, CancellationToken cancellationToken = default)
    {
        var service = await _context.Services
            .FirstOrDefaultAsync(s => s.ServiceId == serviceId, cancellationToken);

        if (service == null)
        {
            return false;
        }

        _context.Services.Remove(service);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    /// Maps a Service entity to a ServiceDto
    private static ServiceDto MapToDto(Service service, Business business)
    {
        return new ServiceDto(
            service.ServiceId,
            service.BusinessId,
            business.Name,
            service.Name,
            service.Description,
            service.DurationMinutes,
            service.Price,
            service.IsActive);
    }
}
