using Application.Dtos;

namespace Application.Interfaces;

public interface IAlarmService
{
    Task<AlarmResponse?> AddAlarmAsync(CreateAlarm createAlarm);
    Task<AlarmResponse?> GetAlarmByIdAsync(int alarmId);
    Task<AlarmResponse?> GetAlarmByMqttTopicAsync(string mqttTopic);
    Task<List<AlarmResponse>> GetAllAlarmsForLocationAsync(int locationId);
    Task<List<AlarmResponse>> GetAllAlarmsAsync();
    Task UpdateAlarmAsync(CreateAlarm updateAlarm);
    Task DeleteAlarmAsync(int alarmId);
    Task SetAlarmStatusAsync(int alarmId, bool status);
}