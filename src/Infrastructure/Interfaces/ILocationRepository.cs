using Domain.Models;


namespace Infrastructure.Interfaces;

public interface ILocationRepository
{
    Task<Location> AddLocationAsync(Location location);
    Task<Location?> GetLocationByIdAsync(int locationId);
    Task<List<Location>> GetAllLocationsAsync();
    Task UpdateLocationAsync(Location location);
    Task DeleteLocationAsync(int locationId);
}
