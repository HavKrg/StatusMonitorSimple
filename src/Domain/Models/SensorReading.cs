namespace Domain.Models;

public class SensorReading : BaseEntity
{
    public SensorReading(Guid sensorId, double value)
    {
        SensorId = sensorId;
        Value = value;
    }

    public Guid SensorId { get; set; }
    public double Value { get; set; }
}