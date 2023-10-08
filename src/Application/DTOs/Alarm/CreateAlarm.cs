namespace Application.Dtos;

public class CreateAlarm
{
    public int Id { get; set; }
    public int LocationId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string MqttTopic { get; set; }
    public required int SecondsBeforeOldReading { get; set; }
    public bool? Status { get; set; } = null;
    public bool Inverted { get; set; }

    public static explicit operator Alarm(CreateAlarm createAlarm)
    {
        return new Alarm(
            createAlarm.Id,
            createAlarm.LocationId,
            createAlarm.Name, 
            createAlarm.Description, 
            createAlarm.MqttTopic, 
            createAlarm.SecondsBeforeOldReading,
            createAlarm.Status,
            createAlarm.Inverted);
    }
}