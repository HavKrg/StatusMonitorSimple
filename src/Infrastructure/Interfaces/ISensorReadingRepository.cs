using Domain.Models;

namespace Infrastructure.Interfaces;

public interface ISensorReadingRepository
{
    Task<bool> AddReadingAsync(SensorReading sensorReading);
    Task<SensorReading?> GetLatestReadingForSensorAsync(int sensorId);
    Task<PaginatedData<List<SensorReading>>?> GetPaginatedSensorReadings(int sensorId, int pageNumber);

}
