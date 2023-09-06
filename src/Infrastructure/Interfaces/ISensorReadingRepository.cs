using Domain.Models;

namespace Infrastructure;

public interface ISensorReadingRepository
{
    Task<bool> AddReadingAsync(SensorReading sensorReading);
    Task<SensorReading?> GetLatestReadingForSensorAsync(Guid sensorId);
}
