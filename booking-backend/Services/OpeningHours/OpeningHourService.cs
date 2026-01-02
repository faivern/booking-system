using Microsoft.EntityFrameworkCore;
using booking_backend.Data;
using booking_backend.DTOs.OpeningHours;
using booking_backend.Models;

namespace booking_backend.Services.OpeningHours;

/// <summary>
/// Service for managing opening hours
/// </summary>
public class OpeningHourService : IOpeningHourService
{
    private readonly BookingSystemDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="OpeningHourService"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public OpeningHourService(BookingSystemDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates a new opening hour for a business
    /// </summary>
    public async Task<OpeningHourDto> CreateOpeningHourAsync(CreateOpeningHourDto request, CancellationToken cancellationToken = default)
    {
        // Validate day of week (0-6: Sunday-Saturday)
        if (request.DayOfWeek < 0 || request.DayOfWeek > 6)
        {
            throw new ArgumentException("DayOfWeek must be between 0 (Sunday) and 6 (Saturday)");
        }

        // Validate time range
        if (request.EndTime <= request.StartTime)
        {
            throw new ArgumentException("EndTime must be after StartTime");
        }

        // Verify business exists
        var business = await _context.Businesses
            .FirstOrDefaultAsync(b => b.BusinessId == request.BusinessId, cancellationToken);

        if (business == null)
        {
            throw new InvalidOperationException($"Business {request.BusinessId} not found");
        }

        var openingHour = new OpeningHour
        {
            BusinessId = request.BusinessId,
            DayOfWeek = request.DayOfWeek,
            StartTime = request.StartTime,
            EndTime = request.EndTime
        };

        _context.OpeningHours.Add(openingHour);
        await _context.SaveChangesAsync(cancellationToken);

        return MapToDto(openingHour, business);
    }

    /// <summary>
    /// Retrieves an opening hour by ID
    /// </summary>
    public async Task<OpeningHourDto?> GetOpeningHourByIdAsync(int openingHourId, CancellationToken cancellationToken = default)
    {
        var openingHour = await _context.OpeningHours
            .Include(oh => oh.Business)
            .FirstOrDefaultAsync(oh => oh.OpeningHourId == openingHourId, cancellationToken);

        return openingHour == null ? null : MapToDto(openingHour, openingHour.Business!);
    }

    /// <summary>
    /// Retrieves all opening hours for a business, sorted by day and start time
    /// </summary>
    public async Task<IEnumerable<OpeningHourDto>> GetBusinessOpeningHoursAsync(int businessId, CancellationToken cancellationToken = default)
    {
        var openingHours = await _context.OpeningHours
            .Where(oh => oh.BusinessId == businessId)
            .Include(oh => oh.Business)
            .OrderBy(oh => oh.DayOfWeek)
            .ThenBy(oh => oh.StartTime)
            .ToListAsync(cancellationToken);

        return openingHours.Select(oh => MapToDto(oh, oh.Business!));
    }

    /// <summary>
    /// Updates an existing opening hour
    /// </summary>
    public async Task<OpeningHourDto?> UpdateOpeningHourAsync(int openingHourId, UpdateOpeningHourDto request, CancellationToken cancellationToken = default)
    {
        var openingHour = await _context.OpeningHours
            .Include(oh => oh.Business)
            .FirstOrDefaultAsync(oh => oh.OpeningHourId == openingHourId, cancellationToken);

        if (openingHour == null)
        {
            return null;
        }

        // Validate day of week (0-6: Sunday-Saturday)
        if (request.DayOfWeek < 0 || request.DayOfWeek > 6)
        {
            throw new ArgumentException("DayOfWeek must be between 0 (Sunday) and 6 (Saturday)");
        }

        // Validate time range
        if (request.EndTime <= request.StartTime)
        {
            throw new ArgumentException("EndTime must be after StartTime");
        }

        openingHour.DayOfWeek = request.DayOfWeek;
        openingHour.StartTime = request.StartTime;
        openingHour.EndTime = request.EndTime;

        _context.OpeningHours.Update(openingHour);
        await _context.SaveChangesAsync(cancellationToken);

        return MapToDto(openingHour, openingHour.Business!);
    }

    /// <summary>
    /// Deletes an opening hour
    /// </summary>
    public async Task<bool> DeleteOpeningHourAsync(int openingHourId, CancellationToken cancellationToken = default)
    {
        var openingHour = await _context.OpeningHours
            .FirstOrDefaultAsync(oh => oh.OpeningHourId == openingHourId, cancellationToken);

        if (openingHour == null)
        {
            return false;
        }

        _context.OpeningHours.Remove(openingHour);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    /// <summary>
    /// Maps an OpeningHour entity to an OpeningHourDto
    /// </summary>
    private static OpeningHourDto MapToDto(OpeningHour openingHour, Business business)
    {
        return new OpeningHourDto(
            openingHour.OpeningHourId,
            openingHour.BusinessId,
            business.Name,
            openingHour.DayOfWeek,
            openingHour.StartTime,
            openingHour.EndTime);
    }
}
