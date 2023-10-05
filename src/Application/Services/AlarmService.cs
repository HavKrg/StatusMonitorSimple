using Application.Dtos;
using Application.Interfaces;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class AlarmService : IAlarmService
{
    private readonly ILogger<AlarmService> _logger;
    private readonly IAlarmRepository _alarmRepository;

    public AlarmService(ILogger<AlarmService> logger, IAlarmRepository alarmRepository)
    {
        _logger = logger;
        _alarmRepository = alarmRepository;
    }

    public async Task<AlarmResponse?> AddAlarmAsync(CreateAlarm createAlarm)
    {
        return await _alarmRepository.AddAlarmAsync((Alarm)createAlarm);
    }

    public async Task DeleteAlarmAsync(int alarmId)
    {
        await _alarmRepository.DeleteAlarmAsync(alarmId);
    }

    public async Task<AlarmResponse?> GetAlarmByIdAsync(int alarmId)
    {
        return await _alarmRepository.GetAlarmByIdAsync(alarmId);
    }

    public async Task<AlarmResponse?> GetAlarmByMqttTopicAsync(string mqttTopic)
    {
        var alarm = await _alarmRepository.GetAlarmByMqttTopicAsync(mqttTopic);
        return alarm;
    }

    public async Task<List<AlarmResponse>> GetAllAlarmsForLocationAsync(int locationId)
    {
        var alarms = await _alarmRepository.GetAllAlarmsForLocationAsync(locationId);
        
        return alarms.Select(alarm => (AlarmResponse)alarm).ToList();
    }

    public async Task<List<AlarmResponse>> GetAllAlarmsAsync()
    {
        var alarms = await _alarmRepository.GetAllAlarmsAsync();
        var response = alarms.Select(alarm => (AlarmResponse)alarm).ToList();
        return response;
    }

    public async Task SetAlarmStatusAsync(int alarmId, bool status)
    {
        await _alarmRepository.SetAlarmStatusAsync(alarmId, status);
    }

    public async Task UpdateAlarmAsync(CreateAlarm updateAlarm)
    {
        await _alarmRepository.UpdateAlarmAsync((Alarm)updateAlarm);
    }
}