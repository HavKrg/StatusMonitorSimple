
using Domain.Models;
using Infrastructure;

namespace Application;

public class SensorReadingService : ISensorReadingService
{
    private readonly ISensorReadingRepository _sensorReadingRepository;

    public SensorReadingService(ISensorReadingRepository sensorReadingRepository)
    {
        _sensorReadingRepository = sensorReadingRepository;
    }

    public async Task<bool> AddReadingAsync(CreateSensorReading createSensorReading)
    {
        return await _sensorReadingRepository.AddReadingAsync((SensorReading)createSensorReading);
    }

    public async Task<SensorReadingResponse?> GetLatestReadingForSensorAsync(Guid sensorId)
    {
        var sensorReadingResponse = await _sensorReadingRepository.GetLatestReadingForSensorAsync(sensorId);
        if(sensorReadingResponse == null)
            return null;
        return sensorReadingResponse;
        
    }
}
