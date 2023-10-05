using Application.Dtos;
using Application.Interfaces;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class LocationService : ILocationService
{
    private readonly ILogger<LocationService> _logger;
    private readonly ILocationRepository _locationRepository;

    public LocationService(ILogger<LocationService> logger, ILocationRepository locationRepository)
    {
        _logger = logger;
        _locationRepository = locationRepository;
    }

    public async Task<LocationResponse> AddLocationAsync(CreateLocation createLocation)
    {
        return await _locationRepository.AddLocationAsync((Domain.Models.Location)createLocation);
    }

    public async Task DeleteLocationAsync(int locationId)
    {
        await _locationRepository.DeleteLocationAsync(locationId);
    }

    public async Task<List<LocationResponse>> GetAllLocationsAsync()
    {
        var locations = await _locationRepository.GetAllLocationsAsync();
        return locations.Select(location => (LocationResponse)location).ToList();
        
    }

    public async Task<LocationResponse?> GetLocationByIdAsync(int locationId)
    {
        return await _locationRepository.GetLocationByIdAsync(locationId);
    }

    public async Task UpdateLocationAsync(CreateLocation updateLocation)
    {
        await _locationRepository.UpdateLocationAsync((Domain.Models.Location)updateLocation);
    }
}