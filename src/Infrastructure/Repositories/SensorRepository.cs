using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class SensorRepository : ISensorRepository
{
    private readonly ILogger<SensorRepository> _logger;
    private readonly StatusMonitorDbContext _context;

    public SensorRepository(ILogger<SensorRepository> logger, StatusMonitorDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Sensor> AddSensorAsync(Sensor sensor)
    {
        if(sensor == null)
            throw new ArgumentNullException(nameof(sensor), $"'{nameof(AddSensorAsync)}': Sensor cannot be null");
        await _context.AddAsync(sensor);
        await _context.SaveChangesAsync();
        return sensor;
    }

    public async Task DeleteSensorAsync(int sensorId)
    {
        var sensor = await _context.Sensors.FindAsync(sensorId);
        if (sensor == null)
            throw new KeyNotFoundException($"'{nameof(DeleteSensorAsync)}': No Sensor found with ID '{sensorId}'");

        _context.Sensors.Remove(sensor);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Sensor>> GetAllSensorsForLocationAsync(int locationId)
    {
        return await _context.Sensors.Where(s => s.LocationId == locationId).ToListAsync();
    }

    public async Task<Sensor?> GetSensorByIdAsync(int sensorId)
    {
        var sensor = await _context.Sensors.FindAsync(sensorId);
        if (sensor == null)
            throw new KeyNotFoundException($"'{nameof(GetSensorByIdAsync)}': No Sensor found with ID '{sensorId}'");
        return sensor;

    }

    public async Task UpdateSensorAsync(Sensor sensor)
    {
        var existingSensor = await _context.Sensors.FindAsync(sensor.Id);
        if(existingSensor == null)
            throw new KeyNotFoundException($"'{nameof(GetSensorByIdAsync)}': No Sensor found with ID '{sensor.Id}'");

        _context.Entry(sensor).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task<List<Sensor>> GetAllSensorsAsync()
    {
        return await _context.Sensors.ToListAsync();
    }

    public Task<Sensor?> GetSensorByMqttTopicAsync(string mqttTopic)
    {
        var sensor = _context.Sensors.FirstOrDefaultAsync(s => s.MqttTopic == mqttTopic);
        if (sensor == null)
            throw new KeyNotFoundException($"'{nameof(DeleteSensorAsync)}': No Sensor found with MqttTopic '{mqttTopic}'");
        return sensor;
    }
}
