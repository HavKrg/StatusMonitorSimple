using Infrastructure;

namespace Application;

public interface ISensorReadingService
{
    Task<bool> AddReadingAsync(CreateSensorReading createSensorReading);
    Task<SensorReadingResponse?> GetLatestReadingForSensorAsync(Guid sensorId);
}
