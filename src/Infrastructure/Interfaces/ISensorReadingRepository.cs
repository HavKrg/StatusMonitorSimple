using Domain.Models;

namespace Infrastructure.Interfaces;

public interface ISensorReadingRepository
{
    Task<bool> AddReadingAsync(SensorReading sensorReading);
    Task<SensorReading?> GetLatestReadingForSensorAsync(Guid sensorId);
    Task<PaginatedData<List<SensorReading>>?> GetPaginatedSensorReadings(Guid sensorId, int pageNumber);

}
