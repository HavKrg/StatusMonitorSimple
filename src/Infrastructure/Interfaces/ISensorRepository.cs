using Domain.Models;

namespace Infrastructure.Interfaces;

public interface ISensorRepository
{
    
    Task<Sensor> CreateSensorAsync(Sensor sensor);
    Task<Sensor> GetSensorByIdAsync(int sensorId);
    Task<IEnumerable<Sensor>> GetAllSensorsAsync();
    Task UpdateSensorAsync(Sensor sensor);
    Task DeleteSensorAsync(int sensorId);
}
