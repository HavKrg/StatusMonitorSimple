using Domain.Models;

namespace Application.Dtos;

public class CreateSensor
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string MqttTopic { get; set; }
    public required double MinReading { get; set; }
    public required double MaxReading { get; set; }
    public required string Measurement { get; set; }
    public required string Group {get; set;} = "&nbsp;";
    public required int PageSize { get; set; }
    public required double Divider { get; set; }

    public static explicit operator Sensor(CreateSensor createSensor)
    {
        return new Sensor(
            createSensor.Id,
            createSensor.Name,
            createSensor.Description, 
            createSensor.MqttTopic, 
            createSensor.MinReading, 
            createSensor.MaxReading, 
            createSensor.Measurement,
            createSensor.PageSize,
            createSensor.Group,
            createSensor.Divider
            );
    }
}
