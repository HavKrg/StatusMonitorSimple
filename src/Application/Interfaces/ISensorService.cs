using Application.Dtos;
using Domain.Models;

namespace Application.Interfaces.Services;

public interface ISensorService
{
    Task<SensorResponse> CreateSensorAsync(CreateSensor createSensor);
    Task<SensorResponse> GetSensorByIdAsync(int sensorId);
    Task<IEnumerable<SensorResponse>> GetAllSensorsAsync();
    Task UpdateSensorAsync(CreateSensor updateSensor);
    Task DeleteSensorAsync(int sensorId);
}
