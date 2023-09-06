using Domain.Models;

namespace Application;

public class CreateSensorReading
{
    public required Guid SensorId { get; set; }
    public required double Value { get; set; }

    public static explicit operator SensorReading(CreateSensorReading createSensorReading)
    {
       return new SensorReading(createSensorReading.SensorId, createSensorReading.Value);
    }
}
