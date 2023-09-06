using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class SensorReadingRepository : ISensorReadingRepository
{
    private readonly StatusMonitorDbContext _context;

    public SensorReadingRepository(StatusMonitorDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AddReadingAsync(SensorReading sensorReading)
    {
        bool sensorExists = await _context.Sensors.AnyAsync(s => s.Id == sensorReading.SensorId);
        if (!sensorExists)
            return false;
        
        await _context.SensorReadings.AddAsync(sensorReading);
        var result = await _context.SaveChangesAsync();
        
        return result > 0;
    }

    public async Task<SensorReading?> GetLatestReadingForSensorAsync(Guid sensorId)
    {
        var response = await _context.SensorReadings
                            .Where(s => s.SensorId == sensorId)
                            .OrderByDescending(s => s.Created)
                            .FirstOrDefaultAsync();

        return response;
    }
}
