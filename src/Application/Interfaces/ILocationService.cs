using Application.Dtos;

namespace Application.Interfaces;

public interface ILocationService
{
    Task<LocationResponse> AddLocationAsync(CreateLocation createLocation);
    Task<LocationResponse?> GetLocationByIdAsync(int locationId);
    Task<List<LocationResponse>> GetAllLocationsAsync();
    Task UpdateLocationAsync(CreateLocation updateLocation);
    Task DeleteLocationAsync(int locationId);
}