
namespace Domain.Models;
public class Sensor : BaseEntity
{
    public int LocationId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string MqttTopic { get; set; }
    public double MinReading { get; set; }
    public double MaxReading { get; set; }
    public string Measurement { get; set; }
    public int SecondsBeforeOldReading { get; set; }
    public int PageSize { get; set; }
    public string Style { get; set; }
    public string Group { get; set; }
    public double Divider { get; set; }
    public IEnumerable<SensorReading> Readings { get; set; } = new List<SensorReading>();
    public Sensor(int id, int locationId, string name, string description, string mqttTopic, double minReading, double maxReading, 
                    string measurement, int secondsBeforeOldReading, int pageSize, string style, string group, double divider)
    {
        Id = id;
        LocationId = locationId;
        Name = name;
        Description = description;
        MqttTopic = mqttTopic;
        MinReading = minReading;
        MaxReading = maxReading;
        Measurement = measurement;
        SecondsBeforeOldReading = secondsBeforeOldReading;
        PageSize = pageSize;
        Style = style;
        Group = group;
        Divider = divider;
    }

    public void Update(Sensor sensor)
    {
        Name = sensor.Name;
        Description = sensor.Description;
        MqttTopic = sensor.MqttTopic;
        MinReading = sensor.MinReading;
        MaxReading = sensor.MaxReading;
        Measurement = sensor.Measurement;
        SecondsBeforeOldReading = sensor.SecondsBeforeOldReading;
        PageSize = sensor.PageSize;
        Style = sensor.Style;
        Group = sensor.Group;
        Updated = DateTime.Now;
    }

    
}

