using Domain.Models;

public class Alarm : BaseEntity
{
    public int LocationId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string MqttTopic { get; set; }
    public int SecondsBeforeOldReading { get; set; }
    public bool? Status { get; set; } = null;
    public bool Inverted { get; set; }

    public Alarm()
    {
        
    }

    public Alarm(int id, int locationId, string name, string description, string mqttTopic, int secondsBeforeOldReading, bool? status, bool inverted)
    {
        Id = id;
        LocationId = locationId;
        Name = name;
        Description = description;
        MqttTopic = mqttTopic;
        SecondsBeforeOldReading = secondsBeforeOldReading;
        Status = null;
        Inverted = inverted;
    }
    public void Update(Alarm alarm)
    {
        LocationId = alarm.LocationId;
        Name = alarm.Name;
        Description = alarm.Description;
        MqttTopic = alarm.MqttTopic;
        SecondsBeforeOldReading = alarm.SecondsBeforeOldReading;
        Status = null;
        Inverted = alarm.Inverted;
        Updated = DateTime.Now;
    }
}