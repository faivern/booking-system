using Microsoft.EntityFrameworkCore;
using booking_backend.Data;
using booking_backend.DTOs.Businesses;
using booking_backend.Models;

namespace booking_backend.Services.Businesses;

/// Service for managing businesses
public class BusinessService : IBusinessService
{
    private readonly BookingSystemDbContext _context;

    public BusinessService(BookingSystemDbContext context)
    {
        _context = context;
    }

    /// Creates a new business
    public async Task<BusinessDto> CreateBusinessAsync(CreateBusinessDto request, CancellationToken cancellationToken = default)
    {
        // Validate required fields
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ArgumentException("Business name is required");
        }

        var business = new Business
        {
            Name = request.Name,
            Description = request.Description,
            Email = request.Email,
            Phone = request.Phone
        };

        _context.Businesses.Add(business);
        await _context.SaveChangesAsync(cancellationToken);

        return MapToDto(business);
    }

    /// Retrieves a business by ID
    public async Task<BusinessDto?> GetBusinessByIdAsync(int businessId, CancellationToken cancellationToken = default)
    {
        var business = await _context.Businesses
            .FirstOrDefaultAsync(b => b.BusinessId == businessId, cancellationToken);

        return business == null ? null : MapToDto(business);
    }

    /// Retrieves all businesses
    public async Task<IEnumerable<BusinessDto>> GetAllBusinessesAsync(CancellationToken cancellationToken = default)
    {
        var businesses = await _context.Businesses
            .OrderBy(b => b.Name)
            .ToListAsync(cancellationToken);

        return businesses.Select(MapToDto);
    }

    /// Updates an existing business
    public async Task<BusinessDto?> UpdateBusinessAsync(int businessId, UpdateBusinessDto request, CancellationToken cancellationToken = default)
    {
        var business = await _context.Businesses
            .FirstOrDefaultAsync(b => b.BusinessId == businessId, cancellationToken);

        if (business == null)
        {
            return null;
        }

        // Validate required fields
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ArgumentException("Business name is required");
        }

        business.Name = request.Name;
        business.Description = request.Description;
        business.Email = request.Email;
        business.Phone = request.Phone;

        _context.Businesses.Update(business);
        await _context.SaveChangesAsync(cancellationToken);

        return MapToDto(business);
    }

    /// Deletes a business
    public async Task<bool> DeleteBusinessAsync(int businessId, CancellationToken cancellationToken = default)
    {
        var business = await _context.Businesses
            .FirstOrDefaultAsync(b => b.BusinessId == businessId, cancellationToken);

        if (business == null)
        {
            return false;
        }

        _context.Businesses.Remove(business);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    /// Maps a Business entity to a BusinessDto
    private static BusinessDto MapToDto(Business business)
    {
        return new BusinessDto(
            business.BusinessId,
            business.Name,
            business.Description,
            business.Email,
            business.Phone);
    }
}
