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
        if(alarm == null)
            throw new ArgumentNullException(nameof(alarm), $"'{nameof(AddAlarmAsync)}': Alarm cannot be null");
        await _context.Alarms.AddAsync(alarm);
        await _context.SaveChangesAsync();
        return alarm;
    }
    public async Task<List<Alarm>> GetAllAlarmsAsync()
    {
        return await _context.Alarms.ToListAsync();
    }

    public async Task<Alarm?> GetAlarmByIdAsync(int alarmId)
    {
        var alarm = await _context.Alarms.FindAsync(alarmId);
        if (alarm == null)
            throw new KeyNotFoundException($"'{nameof(GetAlarmByIdAsync)}': No Alarm found with ID '{alarmId}'");
        return alarm;
    }

    public async Task<List<Alarm>> GetAllAlarmsForLocationAsync(int locationId)
    {
        var alarms = await _context.Alarms.Where(alarm => alarm.LocationId == locationId).ToListAsync();
        return alarms;
    }

    public async Task<Alarm?> GetAlarmByMqttTopicAsync(string mqttTopic)
    {
        var alarm = await _context.Alarms.FirstOrDefaultAsync(a => a.MqttTopic == mqttTopic);
        if (alarm == null)
            throw new KeyNotFoundException($"'{nameof(GetAlarmByMqttTopicAsync)}: No Alarm found with MqttTopic '{mqttTopic}'");

        return alarm;
    }

    public async Task UpdateAlarmAsync(Alarm alarm)
    {
        var existingAlarm = await _context.Alarms.FindAsync(alarm.Id);
        if(existingAlarm == null)
            throw new KeyNotFoundException($"'{nameof(UpdateAlarmAsync)}': No Alarm found with ID '{alarm.Id}'");
            
        _context.Entry(alarm).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAlarmAsync(int alarmId)
    {
        var alarm = await _context.Alarms.FindAsync(alarmId);

        if (alarm == null)
            throw new KeyNotFoundException($"'{nameof(DeleteAlarmAsync)}': No Alarm found with ID '{alarmId}'");

        _context.Alarms.Remove(alarm);
        await _context.SaveChangesAsync();
    }

    public async Task SetAlarmStatusAsync(int alarmId, bool status)
    {
        var alarm = await _context.Alarms.FindAsync(alarmId);
        if (alarm == null)
            throw new KeyNotFoundException($"'{nameof(SetAlarmStatusAsync)}': No Alarm found with ID '{alarmId}'");

        alarm.Status = status;
        alarm.Updated = DateTime.Now;
        await _context.SaveChangesAsync();
    }
}