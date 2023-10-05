using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class LocationRepository : ILocationRepository
{
    private readonly ILogger<LocationRepository> _logger;
    private readonly StatusMonitorDbContext _context;

    public LocationRepository(ILogger<LocationRepository> logger, StatusMonitorDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Location> AddLocationAsync(Location location)
    {
        if(location == null)
            throw new ArgumentNullException(nameof(location), $"'{nameof(AddLocationAsync)}': Location cannot be null");
        await _context.Locations.AddAsync(location);
        await _context.SaveChangesAsync();

        return location;
    }

    public async Task<List<Location>> GetAllLocationsAsync()
    {
        return await _context.Locations.ToListAsync();
    }

    public async Task<Location?> GetLocationByIdAsync(int locationId)
    {
        var location = await _context.Locations.FindAsync(locationId);
        if (location == null)
            throw new KeyNotFoundException($"'{nameof(GetLocationByIdAsync)}': No Location found with ID '{locationId}'");

        return location;
    }

    public async Task UpdateLocationAsync(Location location)
    {
        var existingLocation = await _context.Locations.FindAsync(location.Id);
        if(existingLocation == null)
            throw new KeyNotFoundException($"'{nameof(GetLocationByIdAsync)}': No Location found with ID '{location.Id}'");

        _context.Entry(location).State = EntityState.Modified;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteLocationAsync(int locationId)
    {
        var location = await _context.Locations.FindAsync(locationId);
        if (location == null)
            throw new KeyNotFoundException($"'{nameof(DeleteLocationAsync)}': No Location found with ID '{locationId}'");

        _context.Remove(location);

        await _context.SaveChangesAsync();
    }
}