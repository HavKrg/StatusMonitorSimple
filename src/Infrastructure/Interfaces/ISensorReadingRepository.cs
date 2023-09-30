using Domain.Models;

namespace Infrastructure.Interfaces;

public interface ISensorReadingRepository
{
    Task<SensorReading> AddSensorReadingAsync(SensorReading sensorReading);
    Task<SensorReading?> GetSensorReadingByIdAsync(int sensorReadingId);
    Task<IEnumerable<SensorReading>> GetAllSensorReadingsForSensorAsync(int sensorId);
    Task<SensorReading?> GetLatestReadingForSensorAsync(int sensorId);
    Task<PaginatedData<List<SensorReading>>?> GetPaginatedSensorReadings(int sensorId, int pageNumber);
}
