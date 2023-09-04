using Application.Interfaces.Services;
using Domain.Models;
using Infrastructure.Interfaces;

namespace Application.Services;

public class SensorService : ISensorService
{
    private readonly ISensorRepository _sensorRespository;

    public SensorService(ISensorRepository sensorRespository)
    {
        _sensorRespository = sensorRespository;
    }

    public async Task<Sensor> CreateSensorAsync(Sensor sensor)
    {
        return await _sensorRespository.CreateSensorAsync(sensor);
    }

    public async Task<IEnumerable<Sensor>> GetAllSensorsAsync()
    {
        return await _sensorRespository.GetAllSensorsAsync();
    }

    public async Task<Sensor> GetSensorByIdAsync(Guid sensorId)
    {
        return await _sensorRespository.GetSensorByIdAsync(sensorId);
    }

    public async Task UpdateSensorAsync(Sensor sensor)
    {
        await _sensorRespository.UpdateSensorAsync(sensor);
    }
    public async Task DeleteSensorAsync(Guid sensorId)
    {
        await _sensorRespository.DeleteSensorAsync(sensorId);
    }
}
