using Domain.Models;

namespace Application;

public class CreateSensor
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string MqttTopic { get; set; }
    public required int MinReading { get; set; }
    public required int MaxReading { get; set; }
    public required string Measurement { get; set; }

    public static explicit operator Sensor(CreateSensor createSensor)
    {
        return new Sensor(
            createSensor.Id,
            createSensor.Name,
            createSensor.Description, 
            createSensor.MqttTopic, 
            createSensor.MinReading, 
            createSensor.MaxReading, 
            createSensor.Measurement
            );
    }
}
