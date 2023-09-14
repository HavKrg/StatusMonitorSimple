using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class SensorRepository : ISensorRepository
{
    private readonly StatusMonitorDbContext _context;

    public SensorRepository(StatusMonitorDbContext context)
    {
        _context = context;
    }

    public async Task<Sensor> CreateSensorAsync(Sensor sensor)
    {
        await _context.Sensors.AddAsync(sensor);
        await _context.SaveChangesAsync();
        return sensor;
    }

    public async Task<IEnumerable<Sensor>> GetAllSensorsAsync()
    {
        return await _context.Sensors.ToListAsync();
    }

    public async Task<Sensor?> GetSensorByIdAsync(Guid sensorId)
    {
        return await _context.Sensors.FirstOrDefaultAsync(s => s.Id == sensorId);
    }

    public async Task UpdateSensorAsync(Sensor sensor)
    {
        _context.Entry(sensor).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteSensorAsync(Guid sensorId)
    {
        var sensor = await _context.Sensors.FindAsync(sensorId);
        if (sensor == null)
            return;
        _context.Sensors.Remove(sensor);
        await _context.SaveChangesAsync();
    }
}
