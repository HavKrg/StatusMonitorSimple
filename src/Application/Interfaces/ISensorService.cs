using Application.Dtos;
using Domain.Models;

namespace Application.Interfaces.Services;

public interface ISensorService
{
    Task<SensorResponse> AddSensorAsync(CreateSensor createSensor);
    Task<SensorResponse?> GetSensorByIdAsync(int sensorId);
    Task<SensorResponse?> GetSensorByMqttTopicAsync(string mqttTopic);
    Task<IEnumerable<SensorResponse>> GetAllSensorsForLocationAsync(int locationId);
    Task<IEnumerable<SensorResponse>> GetAllSensorsAsync();
    Task UpdateSensorAsync(CreateSensor updateSensor);
    Task DeleteSensorAsync(int sensorId);
}
