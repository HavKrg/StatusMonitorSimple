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
        await _context.AddAsync(sensor);
        await _context.SaveChangesAsync();
        return sensor;
    }

    public async Task DeleteSensorAsync(int sensorId)
    {
        var sensor = await _context.Sensors.FindAsync(sensorId);
        _context.Sensors.Remove(sensor);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Sensor>> GetAllSensorsForLocationAsync(int locationId)
    {
        return await _context.Sensors.Where(s => s.LocationId == locationId).ToListAsync();
    }

    public async Task<Sensor?> GetSensorByIdAsync(int sensorId)
    {
        return await _context.Sensors.FindAsync(sensorId);
    }

    public async Task UpdateSensorAsync(Sensor sensor)
    {
        _context.Entry(sensor).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Sensor>> GetAllSensorsAsync()
    {
        return await _context.Sensors.ToListAsync();
    }

    public Task<Sensor?> GetSensorByMqttTopicAsync(string mqttTopic)
    {
        return _context.Sensors.FirstOrDefaultAsync(s => s.MqttTopic == mqttTopic);
    }
}
