namespace Domain.Models;
public class Sensor : BaseEntity
{
    public Sensor(string name, string description, string mqttTopic, double minReading, double maxReading, string measurement)
    {
        Name = name;
        Description = description;
        MqttTopic = mqttTopic;
        MinReading = minReading;
        MaxReading = maxReading;
        Measurement = measurement;
    }

    public string Name { get; set; }
    public string Description { get; set; }
    public string MqttTopic { get; set; }
    public double MinReading { get; set; }
    public double MaxReading { get; set; }
    public string Measurement { get; set; }
    public IEnumerable<SensorReading> Readings { get; set; } = new List<SensorReading>();
}

