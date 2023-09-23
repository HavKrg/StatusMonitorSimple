
namespace Domain.Models;
public class Sensor : BaseEntity
{

    public string Name { get; set; }
    public string Description { get; set; }
    public string MqttTopic { get; set; }
    public double MinReading { get; set; }
    public double MaxReading { get; set; }
    public string Measurement { get; set; }
    public int PageSize { get; set; }
    public string Group { get; set; }
    public IEnumerable<SensorReading> Readings { get; set; } = new List<SensorReading>();
    public Sensor(int id, string name, string description, string mqttTopic, double minReading, double maxReading, string measurement, int pageSize, string group)
    {
        Id = id;
        Name = name;
        Description = description;
        MqttTopic = mqttTopic;
        MinReading = minReading;
        MaxReading = maxReading;
        Measurement = measurement;
        PageSize = pageSize;
        Group = group;
    }

    public void Update(Sensor sensor)
    {
        Name = sensor.Name;
        Description = sensor.Description;
        MqttTopic = sensor.MqttTopic;
        MinReading = sensor.MinReading;
        MaxReading = sensor.MaxReading;
        Measurement = sensor.Measurement;
        PageSize = sensor.PageSize;
        Group = sensor.Group;
        Updated = DateTime.Now;
    }

    
}

