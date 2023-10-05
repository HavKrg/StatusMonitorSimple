namespace Infrastructure.Interfaces;
public interface IAlarmRepository
{
    Task<Alarm> AddAlarmAsync(Alarm alarm);
    Task<Alarm?> GetAlarmByIdAsync(int alarmId);
    Task<Alarm?> GetAlarmByMqttTopicAsync(string mqttTopic);
    Task<List<Alarm>> GetAllAlarmsForLocationAsync(int locationId);
    Task<List<Alarm>> GetAllAlarmsAsync();
    Task UpdateAlarmAsync(Alarm alarm);
    Task DeleteAlarmAsync(int alarmId);
    Task SetAlarmStatusAsync(int alarmId, bool status);
}