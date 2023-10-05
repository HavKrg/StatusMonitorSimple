using Application.Dtos;
using Application.Interfaces.Services;
using Domain.Models;
using Infrastructure;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using SQLitePCL;

namespace Application.Services;

public class SensorService : ISensorService
{
    private readonly ILogger<SensorService> _logger;
    private readonly ISensorRepository _sensorRespository;
    private readonly ISensorReadingRepository _sensorReadingRepository;

    public SensorService(ILogger<SensorService> logger, ISensorRepository sensorRespository, ISensorReadingRepository sensorReadingRepository)
    {
        _logger = logger;
        _sensorRespository = sensorRespository;
        _sensorReadingRepository = sensorReadingRepository;
    }

    public async Task<SensorResponse> AddSensorAsync(CreateSensor createSensor)
    {
        return await _sensorRespository.AddSensorAsync((Sensor)createSensor);
    }

    public Task DeleteSensorAsync(int sensorId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<SensorResponse>> GetAllSensorsAsync()
    {
        var sensors = await _sensorRespository.GetAllSensorsAsync();
        return sensors.Select(sensor => (SensorResponse)sensor).ToList();
    }

    public async Task<List<SensorResponse>> GetAllSensorsForLocationAsync(int locationId)
    {
        var sensors = await _sensorRespository.GetAllSensorsForLocationAsync(locationId);

        var response = sensors.Select(sensor => (SensorResponse)sensor).ToList();
        foreach (var sensor in response)
        {
            sensor.LatestReading = await _sensorReadingRepository.GetLatestReadingForSensorAsync(sensor.Id);
        }
        return response;
    }

    public async Task<SensorResponse?> GetSensorByIdAsync(int sensorId)
    {
        return await _sensorRespository.GetSensorByIdAsync(sensorId);
    }

    public async Task<SensorResponse?> GetSensorByMqttTopicAsync(string mqttTopic)
    {
        return await _sensorRespository.GetSensorByMqttTopicAsync(mqttTopic);
    }

    public async Task UpdateSensorAsync(CreateSensor updateSensor)
    {
        await _sensorRespository.UpdateSensorAsync((Sensor)updateSensor);
    }
}
