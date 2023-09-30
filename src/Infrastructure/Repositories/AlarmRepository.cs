using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class AlarmRepository : IAlarmRepository
{
    private readonly ILogger<AlarmRepository> _logger;
    private readonly StatusMonitorDbContext _context;

    public AlarmRepository(ILogger<AlarmRepository> logger, StatusMonitorDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Alarm> AddAlarmAsync(Alarm alarm)
    {
        await _context.Alarms.AddAsync(alarm);
        await _context.SaveChangesAsync();
        return alarm;
    }

    public async Task DeleteAlarmAsync(int alarmId)
    {
        var alarm = await _context.Alarms.FindAsync(alarmId);

        if (alarm != null)
            _context.Remove(alarm);

        await _context.SaveChangesAsync();
    }

    public async Task<Alarm?> GetAlarmByIdAsync(int alarmId)
    {
        return await _context.Alarms.FindAsync(alarmId);

    }

    public async Task<IEnumerable<Alarm>> GetAllAlarmsForLocationAsync(int locationId)
    {
        return await _context.Alarms.Where(a => a.LocationId == locationId).ToListAsync();
    }

    public async Task<IEnumerable<Alarm>> GetAllAlarmsAsync()
    {
        return await _context.Alarms.ToListAsync();
    }

    public async Task UpdateAlarmAsync(Alarm alarm)
    {
        _context.Entry(alarm).State = EntityState.Modified;

        await _context.SaveChangesAsync();
    }

    public async Task SetAlarmStatusAsync(int alarmId, bool status)
    {
        var alarm = await _context.Alarms.FindAsync(alarmId);
        if (alarm == null)
        {
            throw new ArgumentException($"No alarm found with ID {alarmId}", nameof(alarmId));
        }

        alarm.Status = status;
        alarm.Updated = DateTime.Now;
        await _context.SaveChangesAsync();
    }

    public async Task<Alarm?> GetAlarmByMqttTopicAsync(string mqttTopic)
    {
        return await _context.Alarms.FirstOrDefaultAsync(a => a.MqttTopic == mqttTopic);
        
    }
}