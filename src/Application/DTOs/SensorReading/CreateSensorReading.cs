using Domain.Models;

namespace Application.Dtos;

public class CreateSensorReading
{
    public required int SensorId { get; set; }
    public required double Value { get; set; }

    public static explicit operator SensorReading(CreateSensorReading createSensorReading)
    {
       return new SensorReading(createSensorReading.SensorId, createSensorReading.Value);
    }
}
