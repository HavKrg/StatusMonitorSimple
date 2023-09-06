using Domain.Models;

namespace Application;

public class SensorResponse
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string MqttTopic { get; set; }
    public required double MinReading { get; set; }
    public required double MaxReading { get; set; }
    public required string Measurement { get; set; }
    public SensorReadingResponse? LatestReading { get; set; }

    public static implicit operator SensorResponse?(Sensor? sensor)
    {
        if(sensor == null)
            return null;
        return new SensorResponse
        {
            Id = sensor.Id,
            Name = sensor.Name,
            Description = sensor.Description,
            MqttTopic = sensor.MqttTopic,
            MinReading = sensor.MinReading,
            MaxReading = sensor.MaxReading,
            Measurement = sensor.Measurement
        };
    }
}
