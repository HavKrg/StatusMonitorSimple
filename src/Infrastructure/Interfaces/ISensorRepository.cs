using Domain.Models;

namespace Infrastructure.Interfaces;

public interface ISensorRepository
{
    
    Task<Sensor> AddSensorAsync(Sensor sensor);
    Task<Sensor?> GetSensorByIdAsync(int sensorId);
    Task<Sensor?> GetSensorByMqttTopicAsync(string mqttTopic);
    Task<List<Sensor>> GetAllSensorsForLocationAsync(int locationId);
    Task<List<Sensor>> GetAllSensorsAsync();
    Task UpdateSensorAsync(Sensor sensor);
    Task DeleteSensorAsync(int sensorId);
}
