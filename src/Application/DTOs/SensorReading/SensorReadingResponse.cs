using Domain.Models;

namespace Application;

public class SensorReadingResponse
{
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public required Guid SensorId { get; set; }
    public required double Value { get; set; }

    public static implicit operator SensorReadingResponse?(SensorReading? sensorReading)
    {
        if(sensorReading == null)
            return null;
            
        return new SensorReadingResponse
        {
            Created = sensorReading.Created,
            Updated = sensorReading.Updated,
            SensorId = sensorReading.SensorId,
            Value = sensorReading.Value
        };
    }
}
