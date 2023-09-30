using Domain.Models;

namespace Application.Dtos;

public class SensorReadingResponse
{
    public int Id { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public required int SensorId { get; set; }
    public required double Value { get; set; }
    public required bool IsValid { get; set; }

    public static implicit operator SensorReadingResponse?(SensorReading? sensorReading)
    {
        if(sensorReading == null)
            return null;
            
        return new SensorReadingResponse
        {
            Id = sensorReading.Id,
            Created = sensorReading.Created,
            Updated = sensorReading.Updated,
            SensorId = sensorReading.SensorId,
            Value = sensorReading.Value,
            IsValid = sensorReading.IsValid
        };
    }
}
