using Application.Dtos;
using Application.Interfaces.Services;
using Domain.Models;
using Infrastructure;
using Infrastructure.Interfaces;

namespace Application.Services;

public class SensorService : ISensorService
{
    private readonly ISensorRepository _sensorRespository;
    private readonly ISensorReadingRepository _sensorReadingRepository;

    public SensorService(ISensorRepository sensorRespository, ISensorReadingRepository sensorReadingRepository)
    {
        _sensorRespository = sensorRespository;
        _sensorReadingRepository = sensorReadingRepository;
    }

    public async Task<SensorResponse> CreateSensorAsync(CreateSensor createSensor)
    {
        
        return await _sensorRespository.CreateSensorAsync((Sensor)createSensor);
    }

    public async Task<IEnumerable<SensorResponse>> GetAllSensorsAsync()
    {
        var sensors = await _sensorRespository.GetAllSensorsAsync();

        var response = sensors.Select(sensor => (SensorResponse)sensor).ToList();
        foreach (var sensor in response)
        {
            sensor.LatestReading = await _sensorReadingRepository.GetLatestReadingForSensorAsync(sensor.Id);
        }
        return response;
    }

    public async Task<SensorResponse> GetSensorByIdAsync(Guid sensorId)
    {
        return await _sensorRespository.GetSensorByIdAsync(sensorId);
    }

    public async Task UpdateSensorAsync(CreateSensor updateSensor)
    {
        await _sensorRespository.UpdateSensorAsync((Sensor)updateSensor);
    }
    public async Task DeleteSensorAsync(Guid sensorId)
    {
        await _sensorRespository.DeleteSensorAsync(sensorId);
    }

}
