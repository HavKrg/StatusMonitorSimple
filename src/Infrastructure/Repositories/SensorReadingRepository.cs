using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class SensorReadingRepository : ISensorReadingRepository
{
    private readonly ILogger<SensorReadingRepository> _logger;
    private readonly StatusMonitorDbContext _context;

    public SensorReadingRepository(ILogger<SensorReadingRepository> logger, StatusMonitorDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<SensorReading> AddSensorReadingAsync(SensorReading sensorReading)
    {
        await  _context.SensorReadings.AddAsync(sensorReading);
        await _context.SaveChangesAsync();

        return sensorReading;
    }

    public async Task<IEnumerable<SensorReading>> GetAllSensorReadingsForSensorAsync(int sensorId)
    {
        return await _context.SensorReadings.Where(a => a.SensorId == sensorId).ToListAsync();
    }

    public async Task<SensorReading?> GetLatestReadingForSensorAsync(int sensorId)
    {
        return await _context.SensorReadings.OrderByDescending(s => s.Created).Where(s => s.SensorId == sensorId).FirstOrDefaultAsync();
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

    public async Task<SensorReading?> GetSensorReadingByIdAsync(int sensorReadingId)
    {
        return await _context.SensorReadings.FindAsync(sensorReadingId);
    }
}
