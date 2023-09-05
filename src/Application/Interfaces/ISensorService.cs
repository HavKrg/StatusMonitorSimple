using Domain.Models;

namespace Application.Interfaces.Services;

public interface ISensorService
{
    Task<Sensor> CreateSensorAsync(CreateSensor createSensor);
    Task<Sensor> GetSensorByIdAsync(Guid sensorId);
    Task<IEnumerable<Sensor>> GetAllSensorsAsync();
    Task UpdateSensorAsync(Sensor sensor);
    Task DeleteSensorAsync(Guid sensorId);
}
