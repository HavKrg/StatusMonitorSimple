using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Infrastructure.Repositories;

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

    public async Task<IEnumerable<SensorReading>> GetAllReadingsForSensorAsync(int sensorId)
    {
        return await _context.SensorReadings.Where(s => s.SensorId == sensorId).OrderBy(s => s.Created).ToListAsync();

    }

    public async Task<SensorReading?> GetLatestReadingForSensorAsync(int sensorId)
    {
        var response = await _context.SensorReadings
                            .Where(s => s.SensorId == sensorId)
                            .OrderByDescending(s => s.Created)
                            .FirstOrDefaultAsync();

        return response;
    }

    public async Task<PaginatedData<List<SensorReading>>?> GetPaginatedSensorReadings(int sensorId, int pageNumber)
    {
        var sensor = await _context.Sensors.FindAsync(sensorId);

        if (sensor == null)
            return null;

        var readings = await _context.SensorReadings.Where(s => s.SensorId == sensorId).ToListAsync();

        var totalPages = (int)Math.Ceiling((double)readings.Count() / sensor.PageSize);

        if (pageNumber > totalPages || pageNumber == 0)
            pageNumber = totalPages;


        var result = new PaginatedData<List<SensorReading>>(pageNumber, totalPages, sensor.PageSize);
        

        if(pageNumber == totalPages)
        {
            result.Data = readings
                                .TakeLast(result.PageSize)
                                .ToList();
        }
        else
        {
            result.Data = readings
                                .Skip((pageNumber - 1) * result.PageSize)
                                .Take(result.PageSize)
                                .ToList();
        }

        return result;
    }   
}
